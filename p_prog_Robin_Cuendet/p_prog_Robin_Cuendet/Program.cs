///ETML
///Auteur : Robin Cuendet
///Date     29.08.2025
///Description : 


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace p_prog_SecretCode__RobinCuendet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            string restart = "o", usrNumber;
            bool won = false, doubleDigits = false ;
            byte level = 0;
            int tries = 1, maxNum;
            int[] secretCode = new int[4], digits = new int[4], allTriedNum = new int[10];

            do
            {
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

                    if (!byte.TryParse(Console.ReadLine(), out level) || level < 1 || level > 4)
                    {
                        Console.WriteLine("Choix invalide. Essai encore");
                        level = 0;
                    }
                } while (level == 0);

                Console.Clear();


                //choisit un nombre max du code secret selon le niveau
                if (level < 3)
                {
                    maxNum = 7;
                }
                else if (level == 3)
                {
                    maxNum = 9;
                }
                else
                {
                    maxNum = 10;
                }

                //crée le code secret
                secretCode = SetCode(maxNum, level);

                //change le titre de la console pour le code secret
                Console.Title = "Code Secret : " + secretCode[0] + secretCode[1] + secretCode[2] + secretCode[3];
                do
                {
                    doubleDigits = false;
                    Console.Clear();

                    Console.WriteLine("=== SECRET CODE NIVEAU {0} ===\nEssais :\n\n", level);
                    

                    for (int i = 1; i < tries-1; i++)
                    {

                            Console.WriteLine(" {0}: {1}\n\n", i, allTriedNum[i]);
                    }

                    Console.Write("Essai {0}/10 :\nEntre 4 chiffres entre 1 et {1} (ex: 1234) : ", tries, maxNum - 1);

                    //si le code n'est pas un nombre ou que le nombre est en dehors des limites, message d'erreur, sinon je normal
                    usrNumber = Console.ReadLine();


                    for (int i = 0; i < usrNumber.Length; i++)
                    {
                        digits[i] = usrNumber[i] - '0';
                    }

                    for (int i = 0; i < usrNumber.Length; i++)
                    {
                        if (digits[i] >= maxNum)
                        {
                            Console.WriteLine("tu ne peux pas entrer un nombre plus grand que la limite");
                            doubleDigits = true;
                            break;
                        }

                        if (Array.IndexOf(digits, digits[i], 0,i) >= 0 && level < 3)
                        {
                            doubleDigits = true;
                            Console.WriteLine("Merci de ne pas entrer de doublons!");
                            break;
                        }
                    }

                    if (int.TryParse(usrNumber, out _) && usrNumber.Length == 4 && !doubleDigits)
                    {
                        tries++;
                        won = CheckEnteredNumber(digits, secretCode, tries, level, won);
                        allTriedNum[tries - 1] = Convert.ToInt32(usrNumber);
                    }
                    else if (!doubleDigits)
                    {
                        Console.WriteLine("entrée incorrecte! essaies de nouveau!");
                    }

                    //si le joueur à rentré le bon nombre il a gagné
                    if (won)
                    {
                        Console.WriteLine("Vous avez gagné");
                        break;
                    }
                    //si le joueur à utilisé ses 10 essais il a perdu
                    else if (tries >= 10)
                    {
                        Console.WriteLine("vous avez perdu, le code était : {0}{1}{2}{3}", secretCode[0], secretCode[1], secretCode[2], secretCode[3]);
                        break;
                    }

                } while (true);

                Console.WriteLine("Voulez-vous recommencer? (o/n)");


                restart = Console.ReadLine();

            }while (restart.ToLower() == "o");

        }
        static int[] SetCode(int numMax, int level)
        {
            int[] code = new int[4];
            Random rnd = new Random();
            for (int i = 0; i <= 3; i++)
            {
                code[i] = rnd.Next(1, numMax);
                if (level == 1 || level == 2)
                {
                    do
                    {
                        code[i] = rnd.Next(1, numMax);
                    } while (Array.IndexOf(code, code[i], 0, i) >= 0);
                }

            }

            return code;
        }


        static bool CheckEnteredNumber(int[] numbers, int[] Code, int tries, byte level, bool won)
        {
            byte[] results = new byte[4];
            int[]allResponses = new int[10];

            //for (int i = 3; i >= 0; i--)
            //{
            //    results[i] = 0;
            //    digits[i] = numbers % 10;
            //    numbers /= 10;
            //}

            for (int i = 0; i < 4; i++)
            {
                if (Array.IndexOf(Code, Code[i], 0, i) != -1)
                {
                    break;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (Code[i] == numbers[i])
                {
                    results[i] = 3;
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (Code.Contains(numbers[i]) && Code[i] != numbers[i])
                        {
                            results[i] = 2;
                        }
                        else
                        {
                            results[i] = 1;
                        }
                    }
                }
               
            }

            if (level == 1 || level == 3)
            {
                WriteAnswerLevel1And3(results);
            }
            else
            {
                WriteAnswerLevel2And4(results);
            }

            for (int i = 0;i < 4; i++)
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
        static void WriteAnswerLevel1And3(byte[]results)
        {
            for (int i = 0; i < 4; i++)
            {
                if (results[i] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }else if (results[i] == 2)
                {
                    Console.ForegroundColor= ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write('■');
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void WriteAnswerLevel2And4(byte[] results)
        {
            byte goods = 0, ok = 0,bads = 0;
            for (int i = 0; i < 4; i++)
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
            Console.WriteLine("→ {0} bien placé(s), {1} mal placé(s), {2} fausse(s)", goods, ok, bads);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
