///ETML
///Auteur : Robin Cuendet
///Date   :  29.08.2025
///Description : jeu dont le but est de trouver un code secret généré aléatoirement


using System;
using System.Linq;

namespace p_prog_SecretCode__RobinCuendet
{
    internal class Program
    {
        const byte CODELENGH = 4;                               //nombre maximal d'essai
        const byte MAXTRIES = 10;                               //longueur du code secret
        static void Main()
        {

            ConsoleKeyInfo cki;
            string restart = "o";                               //caractère rentré par le joueur pour recommencer
            string usrNumber;                                   //code que le joueur va renter pour trouver le code secret en string
                byte level = 0;                                 //difficulté du je choisi par le joueur
                int[] secretCode = new int[CODELENGH];          //code secret que le joueur devra trouver
                int []digits = new int[CODELENGH];              //code que le joueur va renter pour trouver le code secret en int
                string[] allTriedNum = new string[MAXTRIES];    //collection de tout les essais rentré par le joueur
                int tries = 1;                                  //nombre d'essais du joueur
                int maxNum = 0;                                 //chiffre maximal généré dans le code
                bool won = false;                               //dit si le joueur a gagné
                bool wrongNum = true;                           //dit si le joueur a entré un nombre incorrect

                //écrit le titre avec règles
                WriteTitle();

                //attend qu'une touche est entrée pour continuer
                do
                {
                    cki = Console.ReadKey();
                } while (cki == null);

                Console.Clear();

                //écris les instruction du choix de niveaux
                Console.WriteLine("=== SECRET CODE ===\n");
                Console.WriteLine("Choisi un niveau :");
                Console.WriteLine("1. Débutant\t\t(1 à 6, sans doublons, indices visibles");
                Console.WriteLine("2. Intermédiaire\t(1 à 6, sans doublons, indices discrets");
                Console.WriteLine("3. Avancé\t\t(1 à 8, sans doublons, indices visibles");
                Console.WriteLine("4. Expert\t\t(1 à 9, sans doublons, indices discrets\n");


                //boucle qui vérifie que le choix de niveau entré soi en norme
                do
                {
                    Console.Write("Votre choix (1-4)");

                    //si ce n'est pas un nombres entier ou n'est pas entre 1 et 4 message d'erreur
                    if (!byte.TryParse(Console.ReadLine(), out level) || level < 1 || level > 4)
                    {
                        Console.WriteLine("Choix invalide. Essai encore");
                        level = 0;
                    }
                } while (level == 0);

                Console.Clear();


                //choisit un nombre max du code secret selon le niveau
                switch (level)
                {
                    case 1:
                    case 2:
                        maxNum = 7;
                        break;
                    case 3:
                        maxNum = 9;
                        break;
                    case 4:
                        maxNum = 10;
                        break;
                }

                //crée le code secret
                secretCode = SetCode(maxNum, level);

                //change le titre de la console pour le code secret
                Console.Title = "Code Secret : " + secretCode[0] + secretCode[1] + secretCode[2] + secretCode[3];
                Console.WriteLine("=== SECRET CODE NIVEAU {0} ===\nEssais :\n\n", level);
                do
                {
                    //boucle qui demande au joueur d'entrer un nombre tant qu'il ne va pas
                    do
                    {
                        wrongNum = false;

                        Console.Write("Essai {0}/{1} :\nEntre 4 chiffres entre 1 et {2} (ex: 1234) : ", tries, MAXTRIES, maxNum - 1);
                        usrNumber = Console.ReadLine();

                        //s'il n'y a pas qe des nombres ou que le code ne fais pas la bonne longueur chiffres, message d'erreur
                        if (int.TryParse(usrNumber, out _) && usrNumber.Length == CODELENGH)
                        {
                            for (int i = 0; i < CODELENGH; i++)
                            {
                                digits[i] = usrNumber[i] - '0';

                                //si les ^chiffres ne sont pas entre 1 et le nombre max, message d'erreur
                                if (digits[i] >= maxNum || digits[i] <= 0)
                                {
                                    Console.WriteLine("tu ne peux pas entrer un nombre plus petit que 1 ou plus grand que {0}", maxNum - 1);
                                    wrongNum = true;
                                    break;
                                }

                                //s'il y a des doublons et que le niveau est 1 ou 2, message d'erreur
                                if (Array.IndexOf(digits, digits[i], 0, i) >= 0 && level < 3)
                                {
                                    wrongNum = true;
                                    Console.WriteLine("Merci de ne pas entrer de doublons!");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("entrée incorrecte! essaies de nouveau!");
                            wrongNum = true;
                        }
                    } while (wrongNum);

                    Console.Clear();
                    allTriedNum[tries - 1] = usrNumber;
                    Console.WriteLine("=== SECRET CODE NIVEAU {0} ===\n\n\nEssais :\n\n", level);

                    for (int i = 0; i < MAXTRIES && allTriedNum[i] != null; i++)
                    {
                        Console.Write("{0} : \t{1}\n\t", i + 1, allTriedNum[i]);
                        won = CheckEnteredNumber(allTriedNum[i],secretCode , tries, level, won);
                    }

                    //si le joueur à rentré le bon nombre il a gagné
                    if (won)
                    {
                        Console.WriteLine("Vous avez gagné");
                        break;
                    }
                    //si le joueur à utilisé ses 10 essais il a perdu
                    else if (tries >= MAXTRIES)
                    {
                        Console.WriteLine("vous avez perdu, le code était : {0}{1}{2}{3}", secretCode[0], secretCode[1], secretCode[2], secretCode[3]);
                        break;
                    }
                    tries++;

                } while (true);

                Console.WriteLine("Voulez-vous recommencer? (o/n)");

            //attend que le joueur clique sur o ou n, donne un message d'erreur s'il ne le fais pas
            do
            {
                cki = Console.ReadKey();

                if (cki.Key != ConsoleKey.N && cki.Key != ConsoleKey.O)
                    Console.WriteLine("entrée invalide, veuillez recommencer");

            } while (cki.Key != ConsoleKey.N && cki.Key != ConsoleKey.O);

            //si la touche o à été cliquée, on recommance
            if(cki.Key == ConsoleKey.O)
            {
                Main();
            }
        }

        /// <summary>
        /// Affiche le titre du jeu avec les règles
        /// </summary>
        static void WriteTitle()
        {
            Console.Clear();
            //écriture tu titre
            Console.WriteLine("╔════════════════════ Robin Cuendet ════════════════════╗ ");
            Console.WriteLine("║                                                       ║ ");
            Console.WriteLine("║          Bienvenue dans le jeu : Secret Code          ║ ");
            Console.WriteLine("║                                                       ║ ");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝ ");

            //écriture des règle du jeu
            Console.WriteLine("Un code secret composé de 4 chiffres est généré.\nÀ toi de le découvrire en 10 essais maximum !\n");
            Console.WriteLine("À chaques essai, tu reçois un indice selon le niveau choisi\n\nPour les niveaux 1 et 3 avec indices visibles : ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("■");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" : chiffre bien placé");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("■");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" : chiffre correcte et bien placé\n■ : chiffre bien placé\n");
            Console.WriteLine("Exemple :\ncode secret : 1234 (caché)\n votre essai : 1325\nIndice :");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("■");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("■■");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("■ (1 bien placé, 2 mal placé, 1 absent)\n");
            Console.WriteLine("Pour les niveaux 2 et 4 avec indices discrets : \nExemple :\nCode secret : 5413 (caché)\nVotre essai : 1234 \nIndice :");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("→ 0 bien placé(s), 3 mal placé(s)\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Appuie sur une touche pour commencer...\n");
        }
        /// <summary>
        /// Crée le code secret, avec doublons si ne niveau est 1 ou 2
        /// </summary>
        /// <param name="numMax"></param>
        /// <param name="level"></param>
        /// <returns>le code secret</returns>
        static int[] SetCode(int numMax, int level)
        {
            int[] code = new int[CODELENGH];                    //code secret à générer
            Random rnd = new Random();                          //nombre random à générer
            for (int i = 0; i < CODELENGH; i++)
            {
                code[i] = rnd.Next(1, numMax);
                //si le niveau est 1 ou 2, regénérer le code sans doublons
                if (level < 3)
                {
                    //tant qu'il y a des doublons, regénération du code
                    do
                    {
                        code[i] = rnd.Next(1, numMax);
                    } while (Array.IndexOf(code, code[i], 0, i) >= 0);
                }

            }

            return code;
        }
        /// <summary>
        /// Crée un code de résultat selon le code entré et appelle des fonctions qui écrive le résultat
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="Code"></param>
        /// <param name="tries"></param>
        /// <param name="level"></param>
        /// <param name="won"></param>
        /// <returns></returns>
        static bool CheckEnteredNumber(string digits, int[] Code, int tries, byte level, bool won)
        {
            int checkPlace;                                     //index d'un nombre trouvé dans le code et le nombre entré par le joueur
            byte[] results = new byte[CODELENGH];               //code disant quel nombre est juste (3), mal placé (2) ou faux (1) pour la génération de la réponse
            int[] numbers = new int[CODELENGH];                 //tableau représentant le code en int
            int[]codeCopy = new int[CODELENGH];                 //copie du code secret pour la céréfications de nombre

            //copie de Code dans codeCopy
            //initialisation du code (string) en int
            for (int i = 0; i < CODELENGH; i++)
            {
                codeCopy[i] = Code[i];
                numbers[i] = digits[i] - '0';
            }

            //for (int i = 3; i >= 0; i--)
            //{
            //    results[i] = 0;
            //    digits[i] = numbers % 10;
            //    numbers /= 10;
            //}

            for(int i  = 0; i < CODELENGH; i++)
            {
                if (Code[i] == numbers[i])
                {
                    results[i] = 3;
                    codeCopy[i] = 0;
                }
            }


            for (int i = 0; i < CODELENGH; i++)
            {

                checkPlace = Array.IndexOf(codeCopy, numbers[i]);

                if (codeCopy.Contains(numbers[i]) && results[i] != 3)
                {
                    codeCopy[checkPlace] = 0;
                    results[i] = 2;
                }
                else if (results[i] != 3)
                {
                    results[i] = 1;
                }

            }

            //écriture du résultat selon le niveau choisi
            if (level == 1 || level == 3)
            {
                WriteAnswerLevel1And3(results);
            }
            else
            {
                WriteAnswerLevel2And4(results);
            }

            //regarde si tout les nombre rentrés sont justes, si oui, le joueur a gagné
            for (int i = 0; i < CODELENGH; i++)
            {
                if (results[i] == 3)
                {
                    won = true;
                }
                else
                {
                    won = false;
                    break;
                }
            }

            return won;
        }
        /// <summary>
        /// Ecrit les réponses avec les carrés de couleur
        /// </summary>
        /// <param name="results"></param>
        static void WriteAnswerLevel1And3(byte[] results)
        {
            //pour chaque chiffre rentré change la couleur des carrés selon le résultat
            for (int i = 0; i < CODELENGH; i++)
            {
                if (results[i] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (results[i] == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write('■');
            }
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        /// <summary>
        /// Ecris les réponses en disant le nombre de nombre juste, mal placé ou faux
        /// </summary>
        /// <param name="results"></param>
        static void WriteAnswerLevel2And4(byte[] results)
        {
            byte goods = 0, ok = 0, bads = 0;               //nombre de chiffre justes, mal placés et faux dans le code rentré
            //compte le nombre de chiffres justes, mal placés et faux selon le résultat
            for (int i = 0; i < CODELENGH; i++)
            {
                if (results[i] == 1)
                {
                    bads++;
                }
                else if (results[i] == 2)
                {
                    ok++;
                }
                else
                {
                    goods++;
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("→ {0} bien placé(s), {1} mal placé(s), {2} fausse(s)\n", goods, ok, bads);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
