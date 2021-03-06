using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

/*          
 **********************   Rafał Sadowski   **********************    
 **********************      s18311        **********************  
 **********************     Projek NAI     **********************  
 **********************       Nr 1         **********************              
 **********************    Algorytm KNN    **********************  
 */

namespace nai_p1
{
    class Program
    {

        public static readonly string CLOSE_PROGRAM = "exit";
        public static readonly string IRIS_SETOSA = "Iris-setosa";
        public static readonly string IRIS_VERSICOLOR = "Iris-versicolor";
        public static readonly string IRIS_VIRGINICA = "Iris-virginica";

        static void Main(string[] args)
        {


            var daneTreningowe = @"iris_training.txt";
            var daneTestowe = @"iris_test.txt";

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory + @"iris_training.txt");
            int k = 3;

            List<string[]> daneTreningoweRawList = PobierzDane(daneTreningowe);
            List<string[]> daneTestoweRawList = PobierzDane(daneTestowe);

            double[][] daneTreningoweArr = WczytajDaneDouble(daneTreningoweRawList);
            double[][] daneTestoweArr = WczytajDaneDouble(daneTestoweRawList);


            //uczenie



            /*          
             **********************                    **********************    
             **********************                    **********************  
             **********************       KONSOLA      **********************  
             **********************                    **********************              
             **********************                    **********************  
             */

       



            Console.WriteLine("Jesli chcesz sprawdzic dane z pliku iris_test.txt wpisz komende: ");
            Console.WriteLine("test");
            Console.WriteLine(" ");
            Console.WriteLine("Jeśli chcesz podać własny wektor wpisz komende: ");
            Console.WriteLine("wektor");
            Console.WriteLine(" ");
            Console.WriteLine("Aby zakonczyc dzialanie programu wpisz komende ");
            Console.WriteLine("exit");
            Console.WriteLine(" ");

            string comand = " ";
            while ((comand = Console.ReadLine()) != CLOSE_PROGRAM)
            {
                if (comand == "test")
                {
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");
                    Console.WriteLine("Klasyfikacja danych testowych");
                    Console.WriteLine(" ");
                    Console.WriteLine(" ");

                    int licznikPoprawnychKwalifikacji = 0;

                    for (int i = 0; i < daneTestoweArr.Length; i++)
                    {
                        int tmpInt = OkreslGatunekIrisaSprwadzajacPoprawnosc(daneTreningoweRawList, daneTreningoweArr, daneTestoweRawList, daneTestoweArr, daneTestoweArr[i], k);
                        licznikPoprawnychKwalifikacji = licznikPoprawnychKwalifikacji + tmpInt;
                        Console.WriteLine(" ");
                        Console.WriteLine("****************************");
                        Console.WriteLine("****************************");
                        Console.WriteLine("****************************");
                        Console.WriteLine(" ");
                    }

                    double dokladnoscEksperymentu = ((double)licznikPoprawnychKwalifikacji / (double)daneTestoweArr.Length) * 100;

                    Console.WriteLine("------------------------------ ");
                    Console.WriteLine(" ");
                    Console.WriteLine("DOKŁADNOŚĆ EKSPERYMENTU ");
                    Console.WriteLine(dokladnoscEksperymentu + "%");
                    Console.WriteLine(" ");
                    Console.WriteLine("------------------------------");
                }

                if (comand == "wektor")
                {
                    Console.WriteLine("Prosze pamietac ze wektor musi byc podany zgodnie z liczba atrybutow w pliku treningowym");
                    Console.WriteLine("Akceptowalny format:  x,y np 2,5");
                    double[] wektorUzytkownika = new double[daneTreningoweArr[0].Length];
                    for (int i = 0; i < wektorUzytkownika.Length; i++)
                    {

                        Console.WriteLine("Podaj atrybut nr: " + (i + 1));
                        string tmpStr = Console.ReadLine();
                        if (tmpStr.Length != 0)
                        {
                            wektorUzytkownika[i] = double.Parse(tmpStr.Replace('.', ','), CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            Console.WriteLine("Nie wprowadzono zadnego atrybutu, zatem jego wartosc zostaje ustawiona na 0 " + i);
                            wektorUzytkownika[i] = 0.0;
                        }

                    }

                    OkreslGatunekIrisa(daneTreningoweRawList, daneTreningoweArr, wektorUzytkownika, k);



                    Console.WriteLine("");
                    Console.WriteLine("Aby wpisac nowy wektor wpisz komende wektor");
                }

                Console.WriteLine("Jesli chcesz sprawdzic dane z pliku iris_test.txt wpisz komende: ");
                Console.WriteLine("test");
                Console.WriteLine(" ");
                Console.WriteLine("Jeśli chcesz podać własny wektor wpisz komende: ");
                Console.WriteLine("wektor");
                Console.WriteLine(" ");
                Console.WriteLine("Aby zakonczyc dzialanie programu wpisz komende ");
                Console.WriteLine("exit");
                Console.WriteLine(" ");

            }
        }


        /*          
         **********************                    **********************    
         **********************                    **********************  
         **********************       METODY       **********************  
         **********************                    **********************              
         **********************                    **********************  
         */

        public static void OkreslGatunekIrisa(List<String[]> listaTreningowa, double[][] DaneTreningowe, double[] test, int k)
        {
            double[][] elementyK = PozyskanieK(DaneTreningowe, test, k);
            string nazwaGatunkuWgKNN = NajblizszyElement(listaTreningowa, DaneTreningowe, elementyK);
            Console.WriteLine("Dla danych testowych: " + "");
            PrintArr(test);
            Console.WriteLine("Gatunek to:  " + nazwaGatunkuWgKNN);

        }

        public static int OkreslGatunekIrisaSprwadzajacPoprawnosc(List<String[]> listaTreningowa, double[][] DaneTreningowe, List<String[]> listaTestowa, double[][] DaneTestowe, double[] test, int k)
        {
            double[][] elementyK = PozyskanieK(DaneTreningowe, test, k);
            string nazwaGatunkuWgKNN = NajblizszyElement(listaTreningowa, DaneTreningowe, elementyK);
            Console.WriteLine("Dla danych testowych: " + "");
            PrintArr(test);

            string nazwaGatunkuWgPliku = ZwrocNazweKwiatu(listaTestowa, ZwrocIndexKwiatu(test, DaneTestowe)).Replace(" ", null);

            if (String.Equals(nazwaGatunkuWgKNN, nazwaGatunkuWgPliku))
            {
                Console.WriteLine("Uzyskano poprawna kwalifikacje!");
                Console.WriteLine("Gatunek wg KNN to:  " + nazwaGatunkuWgKNN);
                Console.WriteLine("Gatunek wg Pliku to:  " + nazwaGatunkuWgPliku);
                return 1;
            }
            else
            {
                Console.WriteLine("Nie uzyskano poprawnej kwalifikacji :( ");
                Console.WriteLine("Gatunek wg KNN to:  " + nazwaGatunkuWgKNN);
                Console.WriteLine("Gatunek wg Pliku to:  " + nazwaGatunkuWgPliku);
                return 0;
            }


        }


        public static string NajblizszyElement(List<String[]> listaTreningowa, double[][] DaneTreningowe, double[][] elementyK)
        {

            // wystapienia[0] = Iris-setosa 
            // wystapienia[1] = Iris-versicolor
            // wystapienia[2] = Iris-virginica  
            int[] wystapienia = { 0, 0, 0 };
            int max = 0;
            for (int i = 0; i < elementyK.Length; i++)
            {
                int tmpIndex = ZwrocIndexKwiatu(elementyK[i], DaneTreningowe);
                string tmpIris = ZwrocNazweKwiatu(listaTreningowa, tmpIndex).Replace(" ", null);
                if (String.Equals(IRIS_SETOSA, tmpIris))
                {
                    wystapienia[0]++;
                }
                else if (String.Equals(IRIS_VERSICOLOR, tmpIris))
                {
                    wystapienia[1]++;
                }
                else if (String.Equals(IRIS_VIRGINICA, tmpIris))
                {
                    wystapienia[2]++;
                }
            }

            for (int j = 0; j < wystapienia.Length; j++)
            {
                if (wystapienia[j] > wystapienia[max])
                {
                    max = j;
                }
                else if (wystapienia[j] == wystapienia[max])
                {

                    Random rzutMoneta = new Random();
                    int[] tmpArr = { max, j };
                    int tmp = rzutMoneta.Next(0, 2);

                    max = tmpArr[tmp];
                }
            }


            if (max == 0)
            {
                return IRIS_SETOSA;
            }
            else if (max == 1)
            {
                return IRIS_VERSICOLOR;
            }
            else if (max == 2)
            {
                return IRIS_VIRGINICA;
            }
            else
            {
                return "";
            }
        }


        public static int ZwrocIndexKwiatu(double[] wektor, double[][] dane)
        {
            int tmp = 0;
            for (int i = 0; i < dane.Length; i++)
            {
                if (CzySaTakieSameTablice(wektor, dane[i]))
                {
                    return i;
                }
            }
            return tmp;
        }

        public static string ZwrocNazweKwiatu(List<String[]> lista, int index)
        {
            string[] tmpStrArr = lista[index];
            string nazwa = tmpStrArr[tmpStrArr.Length - 1];

            return nazwa;
        }
        public static List<string[]> PobierzDane(string Path)
        {
            var lines = File.ReadLines(Path);
            List<String[]> tmpList = new List<string[]>();

            foreach (var line in lines)
            {
                string[] tmpArr = line.Split("\t");
                tmpList.Add(tmpArr);
            }

            return tmpList;
        }

        public static double[][] WczytajDaneDouble(List<String[]> lista)
        {
            double[][] arr = new double[lista.Count][];

            for (int i = 0; i < lista.Count; i++)
            {
                string[] tmpStrArr = lista[i];
                double[] tmpDblArr = new double[tmpStrArr.Length - 1];
                for (int j = 0; j < tmpDblArr.Length; j++)
                {

                    tmpDblArr[j] = double.Parse(tmpStrArr[j], CultureInfo.CurrentCulture);
                }
                arr[i] = tmpDblArr;
            }

            return arr;
        }

        public static double Euklides(double[] a, double[] b)
        {
            double result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result = result + Potega(a[i] - b[i], 2);
            }

            return result;
        }

        public static double[][] PozyskanieK(double[][] DaneTreningowe, double[] test, int k)
        {
            double[][] result = new double[k][];
            double[][] daneWzgledemEuk = new double[DaneTreningowe.Length][];

            for (int lmao = 0; lmao < daneWzgledemEuk.Length; lmao++)
            {
                daneWzgledemEuk[lmao] = DaneTreningowe[lmao];
            }

            double[] euklidesValues = new double[DaneTreningowe.Length];

            for (int i = 0; i < euklidesValues.Length; i++)
            {
                euklidesValues[i] = Euklides(DaneTreningowe[i], test);

            }

            for (int a = 0; a < euklidesValues.Length; a++)
            {
                for (int b = 0; b < euklidesValues.Length; b++)
                {

                    if (euklidesValues[a] < euklidesValues[b])
                    {
                        double tmpVal = euklidesValues[a];
                        double[] tmpArr = daneWzgledemEuk[a];
                        euklidesValues[a] = euklidesValues[b];
                        daneWzgledemEuk[a] = daneWzgledemEuk[b];
                        euklidesValues[b] = tmpVal;
                        daneWzgledemEuk[b] = tmpArr;
                    }
                }
            }
            for (int i = 0; i < k; i++)
            {
                result[i] = daneWzgledemEuk[i];

            }

            return result;
        }


        /*          
         **********************                    **********************    
         **********************                    **********************  
         ********************** METODY POMOCNICZE  **********************  
         **********************                    **********************              
         **********************                    **********************  
         */

        public static void PrintArr(string[] arr)
        {
            foreach (var item in arr)
            {
                Console.Write(item);
            }

            Console.WriteLine(" ");
        }

        public static void PrintArr(double[] arr)
        {
            foreach (var item in arr)
            {
                Console.Write(item + " | ");
            }

            Console.WriteLine(" ");
        }

        public static bool CzySaTakieSameTablice(double[] arr1, double[] arr2)
        {

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static double Potega(double a, int b)
        {
            double result = a;
            for (int i = 1; i < b; i++)
            {
                result = result * result;
            }

            return result;
        }


    } //program

} //namespace
