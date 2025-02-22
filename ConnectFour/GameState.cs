namespace ConnectFour;

public class GameState
{

	static GameState()
	{
		CalculerLesPositionsGagnantes();
	}

	/// <summary>
	/// Indique si un joueur a gagné, si le jeu est un match nul, ou si le jeu est en cours
	/// </summary>
	public enum WinState
	{
		No_Winner = 0,
		Player1_Wins = 1,
		Player2_Wins = 2,
		Tie = 3
	}

	/// <summary>
	/// Le joueur dont c'est le tour. Par défaut, le joueur 1 commence.
	/// </summary>
	public int PlayerTurn => TheBoard.Count(x => x != 0) % 2 + 1;

	/// <summary>
	/// Nombre de tours effectués et de pièces jouées jusqu'à présent dans le jeu
	/// </summary>
	public int CurrentTurn { get { return TheBoard.Count(x => x != 0); } }

	public static readonly List<int[]> WinningPlaces = new();

	public static void CalculerLesPositionsGagnantes() 
	{

		// Lignes horizontales
        for (byte row = 0; row < 6; row++)
        {
            // Calcul de l'index de la première colonne de la ligne actuelle
            byte rowCol1 = (byte)(row * 7);
            // Calcul de l'index de la dernière colonne de la ligne actuelle
            byte rowColEnd = (byte)((row + 1) * 7 - 1);
            
            // Initialisation de la variable pour vérifier les colonnes successives
            byte checkCol = rowCol1;
            
            // Tant que checkCol permet de former un groupe de 4 cases consécutives
            while (checkCol <= rowColEnd - 3)
            {
                // Ajout des indices des 4 cases consécutives (groupes de 4) à la liste WinningPlaces
                WinningPlaces.Add(new int[] { 
                    checkCol,              // Première case du groupe
                    (byte)(checkCol + 1),  // Deuxième case du groupe
                    (byte)(checkCol + 2),  // Troisième case du groupe
                    (byte)(checkCol + 3)   // Quatrième case du groupe
                });
                
                // Incrémentation de checkCol pour vérifier le groupe suivant
                checkCol++;
            }
        }

		// Colonnes verticales
        for (byte col = 0; col < 7; col++)
        {
            // Calcul de l'index de la première case dans la colonne actuelle
            byte colRow1 = col;
            // Calcul de l'index de la dernière case dans la colonne actuelle
            byte colRowEnd = (byte)(35 + col);
            
            // Initialisation de la variable pour vérifier les lignes successives
            byte checkRow = colRow1;
            
            // Tant que checkRow permet de former un groupe de 4 cases consécutives
            while (checkRow <= 14 + col)
            {
                // Ajout des indices des 4 cases consécutives (groupes de 4) à la liste WinningPlaces
                WinningPlaces.Add(new int[] {
                    checkRow,              // Première case du groupe
                    (byte)(checkRow + 7),  // Deuxième case du groupe
                    (byte)(checkRow + 14), // Troisième case du groupe
                    (byte)(checkRow + 21)  // Quatrième case du groupe
                });
                
                // Incrémentation de checkRow pour vérifier le groupe suivant dans la même colonne
                checkRow += 7;
            }
        }

		// Diagonale en slash "/"
        for (byte col = 0; col < 4; col++)
        {
            // La colonne de départ doit être entre 0 et 3 pour que la diagonale ait suffisamment d'espace
            byte colRow1 = (byte)(21 + col);
            // Calcul de l'index de la dernière case dans la diagonale
            byte colRowEnd = (byte)(35 + col);
            
            // Initialisation de la variable pour vérifier les positions successives de la diagonale
            byte checkPos = colRow1;
            
            // Tant que checkPos permet de former un groupe de 4 cases consécutives dans une diagonale en slash
            while (checkPos <= colRowEnd)
            {
                // Ajout des indices des 4 cases consécutives (groupes de 4) formant une diagonale en slash
                WinningPlaces.Add(new int[] {
                    checkPos,              // Première case de la diagonale
                    (byte)(checkPos - 6),  // Deuxième case de la diagonale (vers le haut à droite)
                    (byte)(checkPos - 12), // Troisième case de la diagonale
                    (byte)(checkPos - 18)  // Quatrième case de la diagonale
                });
                
                // Incrémentation de checkPos pour vérifier la diagonale suivante
                checkPos += 7;
            }
        }

		// Diagonale en anti-slash "\"
        for (byte col = 0; col < 4; col++)
        {
            // La colonne de départ doit être entre 0 et 3 pour que la diagonale ait suffisamment d'espace
            byte colRow1 = (byte)(0 + col);
            // Calcul de l'index de la dernière case dans la diagonale
            byte colRowEnd = (byte)(14 + col);
            
            // Initialisation de la variable pour vérifier les positions successives de la diagonale
            byte checkPos = colRow1;
            
            // Tant que checkPos permet de former un groupe de 4 cases consécutives dans une diagonale en anti-slash
            while (checkPos <= colRowEnd)
            {
                // Ajout des indices des 4 cases consécutives (groupes de 4) formant une diagonale en anti-slash
                WinningPlaces.Add(new int[] {
                    checkPos,              // Première case de la diagonale
                    (byte)(checkPos + 8),  // Deuxième case de la diagonale (vers le haut à gauche)
                    (byte)(checkPos + 16), // Troisième case de la diagonale
                    (byte)(checkPos + 24)  // Quatrième case de la diagonale
                });
                
                // Incrémentation de checkPos pour vérifier la diagonale suivante
                checkPos += 7;
            }
        }
	}

	/// <summary>
	/// Vérifie l'état du plateau pour un scénario de victoire
	/// </summary>
	/// <returns>0 - aucun gagnant, 1 - joueur 1 gagne, 2 - joueur 2 gagne, 3 - match nul</returns>
	public WinState CheckForWin()
	{

		// Sortie immédiate si moins de 7 pièces ont été jouées
		if (TheBoard.Count(x => x != 0) < 7) return WinState.No_Winner;

		foreach (var scenario in WinningPlaces)
		{

			if (TheBoard[scenario[0]] == 0) continue;

			if (TheBoard[scenario[0]] == 
				TheBoard[scenario[1]] && 
				TheBoard[scenario[1]] == 
				TheBoard[scenario[2]] && 
				TheBoard[scenario[2]] == 
				TheBoard[scenario[3]]) return (WinState)TheBoard[scenario[0]];

		}

		if (TheBoard.Count(x => x != 0) == 42) return WinState.Tie;

		return WinState.No_Winner;

	}

	/// <summary>
	/// Prend le tour actuel et place une pièce dans la colonne demandée (indexée à partir de 0)
	/// </summary>
	/// <param name="column">Colonne indexée à partir de 0 dans laquelle la pièce sera placée</param>
	/// <returns>L'index final du tableau où la pièce se trouve</returns>
	public byte PlayPiece(int column)
	{

		// Vérifie s'il y a déjà une victoire
		if (CheckForWin() != WinState.No_Winner) 
            throw new ArgumentException("Le jeu est terminé");


		// Vérifie si la colonne est pleine
		if (TheBoard[column] != 0) 
            throw new ArgumentException("La colonne est pleine");

		// Dépose la pièce
		var landingSpot = column;
		for (var i = column; i < 42; i += 7)
		{
			if (TheBoard[landingSpot + 7] != 0) break;
			landingSpot = i;
		}

		TheBoard[landingSpot] = PlayerTurn;

		return ConvertLandingSpotToRow(landingSpot);

	}

	public List<int> TheBoard { get; private set; } = new List<int>(new int[42]);

	public void ResetBoard() {
		TheBoard = [.. new int[42]];
	}

	private byte ConvertLandingSpotToRow(int landingSpot)
	{

		return (byte)(Math.Floor(landingSpot / (decimal)7) + 1);

	}

}
