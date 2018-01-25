Projet othello Luca srdjenovic - Axel Bento Da Silva - Gabriel Griesser


Partie IA


La DLL OthelloIA1.dll est à la racine du projet et elle est composé des classes :
	- GameBoard.cs, qui descent de IPlayable et contient la solution d'intéligence artificielle
	- Token.cs, qui permet de facilité la gestion des jetons dans le jeu
	- Tools.cs, qui contient quelque fonctions utiles, notament pour convertir certaines propriétés pour qu'elles correspondent à celles de IPlayable
	
	
Dans GameBoard.cs la fonction ComputeMoveScore, permet de calculer la "puissance" d'un board et va permettre à l'IA de calculer à travèrs la
fonction AlphaBeta, quel est le meilleur coup à jouer, (le meilleur coup selon ComputeMoveScore).

Dans cette fonction nous avons créer une matrice carré de 8x8, qui reférence chaque case du plateau de jeu et leurs pondère une valeure, cette  matrice le voici :
	32	1	16	16	16	16	1	32
	1	1	1	1	1	1	1	1
	16	1	2	2	2	2	1	16
	16	1	2	2	2	2	1	16
	16	1	2	2	2	2	1	16
	16	1	2	2	2	2	1	16
	1	1	1	1	1	1	1	1
	32	1	16	16	16	16	1	32

	
Les coins sont les positions qui valent le plus puisqu'ils ne peuvent pas être modifié, les cases autours valent très peu puisque elles permettent
à l'opposant de peut être prendre un coin, tous comme les cases sur les côtés.
	
Cette matrice est uniquement basé sur nos recherches et notre éxperience du jeu, il en éxiste donc d'autres plus optimisé.

Pour tester l'IA, l'ancez l'application -> NewGame -> 1 vs IA -> Sélectionnez un avatar -> La partie va se lancer une fois que l'IA ait choisis le sien.