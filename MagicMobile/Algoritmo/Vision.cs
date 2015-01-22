using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.Widget;
using System.IO;

namespace MagicMobile.Algoritmo
{
    class Vision
    {                  
        //------------------------- variables ------------------------//
        //imagen de entrada y de salida.
        private Color tono;
        public Bitmap bmp_original;
        public Bitmap bmp_output;
        //ecuaciones de la recta. ... primero se crean la muestra y luego se calcula la recta.
        public int[] recta_Ax = new int[4];
        public int[] recta_Ay = new int[4];
        public int[] recta_Bx = new int[4];
        public int[] recta_By = new int[4];
        public double[] coeficiente_a = new double[4];
        public double[] coeficiente_b = new double[4];
        private Point[,] componentes = new Point[200, 4];
        private int nmuestras = 0;
        private int baseA = 0;
        private int baseB = 0;
        //unias superposición
        public Point[] unias = new Point[5];//punto en el que se debe colocar la uña, punto base.
        public int[] grosor = new int[5];
        //---------------------------- métodos -----------------------//
        //este método cambia a escala de grises una imagen, sirve de prueba
        public Bitmap Convertir(Bitmap imgtemp)
        {
            Bitmap bmp_temp = bmp_original.Copy(Bitmap.Config.Rgb565, true);
            Color color;
            int rojo, verde, azul;
            int media;
            for (int i = 0; i < bmp_original.Width; i++)
            {
                for (int j = 0; j < bmp_original.Height; j++)
                {
                   color = new Color(imgtemp.GetPixel(i, j));
                   rojo = color.R;
                   verde = color.G;
                   azul = color.B;
                   media = (int)((rojo + verde + azul) / 3);
                   color = new Color(media, media, media);
                   bmp_temp.SetPixel(i, j, color);
                }
            }
            return bmp_original;
        }

        //recupera el tono de piel 
        public Color Tonalidad()
        {
            //Esta función calcula la tonalidad de la piel.
            Bitmap bmp_temp = bmp_original;//bmp_original.Copy(Bitmap.Config.Rgb565, true);
            //definimos constantes
            int muestra = 5; //se tomarán pixeles 20 x 20 ... un cudrado, para tomar la tonalidad.
            double partidaX = 0.45; //en la escala del 1 al 100, punto en el que empezará a buscar la tonalidad a lo ancho de la imagen
            double partidaY = 0.70; //en la escala del 1 al 100, punto en el que empezará a buscar la tonalidad a lo largo de la imagen
            int promedioR, promedioB, promedioG;//promedio de todos los valores.
            int sumaR, sumaB, sumaG;//suma de cada color
            int totalPixeles;//número total de píxeles tomado en la muestra.
            int baseX, baseY; //píxeles base
            //empeamos a capturar la tonalidad.
            totalPixeles = 0;
            sumaB = 0; sumaG = 0; sumaR = 0;
            baseX = (int)(bmp_temp.Width * partidaX);
            baseY = (int)(bmp_temp.Height * partidaY);
            for (int i = 0; i < muestra; i++)
            {
                for (int j = 0; j < muestra; j++)
                {
                    tono = new Color(bmp_temp.GetPixel(baseX + i, baseY + j));
                    sumaR += tono.R;
                    sumaG += tono.G;
                    sumaB += tono.B;
                    totalPixeles += 1;
                    bmp_temp.SetPixel(baseX + i, baseY + j, Color.Black);
                }
            }
            promedioB = (int)(sumaB / totalPixeles);
            promedioR = (int)(sumaR / totalPixeles);
            promedioG = (int)(sumaG / totalPixeles);
            tono = new Color(promedioR, promedioG, promedioB);
            return tono;
        }
        /*
        public MemoryStream Superponer(Bitmap original, Bitmap superpuesta, int PosX, int PosY)
        {
            //corrección de las posiciones oficiales.
            PosX = PosX - 20;
            PosY = PosY - superpuesta.Height;
            //
            Bitmap bmPhoto = original.Copy(Bitmap.Config.Rgb565, true);
            //Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //grPhoto.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //Rectangle rec = new Rectangle(0, 0, manito.Width, manito.Height);
            //grPhoto.DrawImage(imgPhoto, rec, rec, GraphicsUnit.Pixel);
            //grPhoto.DrawImage(superpuesta, new Rectangle((int)PosX, (int)PosY, (int)(super.Width) - 5, (int)(super.Height + 5)), new Rectangle(0, 0, superpuesta.Width, superpuesta.Height), GraphicsUnit.Pixel);
            MemoryStream mm = new MemoryStream();
            //bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
            //cerramos todo...
            //manito.Dispose();
            //imgPhoto.Dispose();
            //bmPhoto.Dispose();
            //grPhoto.Dispose();
            return mm;
        }*/

        public Bitmap Piel(Color tono, int tolerancia,Bitmap bmp_temp)
        {
            //Esta funcion en base a la tonalidad hace una discriminación, crea una capa de pura piel
            Color color;
            int rojo, verde, azul;
            int tol = 60;
            int baseH, baseI, baseJ; //son limpiadores zona, superiore, zona izqui y zona derecha (3 zonas).
            double escalaH = 0.3;
            double escalaI = 0.17;
            double escalaJ = 0.83;
            //aquí tenemos que limpiar 3 zonas que no va a estar consideradas en el foco principal de la cámara
            baseH = (int)(bmp_temp.Height * escalaH);
            baseI = (int)(bmp_temp.Width * escalaI);
            baseJ = (int)(bmp_temp.Width * escalaJ);
            //limpieza de negro en las 3 seccciones.
            for (int i = 0; i < bmp_temp.Width ; i++)
            {
                for (int j = 0; j < baseH; j++)
                {
                    bmp_temp.SetPixel(i, j, Color.Black);
                }
            }
            for (int i = 0; i < baseI; i++)
            {
                for (int j = baseH; j < bmp_temp.Height; j++)
                {
                    bmp_temp.SetPixel(i, j, Color.Black);
                }
            }
            for (int i = baseJ; i < bmp_temp.Width; i++)
            {
                for (int j = baseH; j < bmp_temp.Height; j++)
                {
                    bmp_temp.SetPixel(i, j, Color.Black);
                }
            }
            //comenzamos a buscar la tonalidad
            for (int i = baseI; i < baseJ; i++)
            {
                for (int j = baseH; j < bmp_temp.Height; j++)
                {
                    color = new Color(bmp_temp.GetPixel(i, j));
                    rojo = color.R;
                    verde = color.G;
                    azul = color.B;
                    //promedio = (int)((rojo + azul + verde) / 3);
                    //color2 =new Color(promedio, promedio, promedio);
                    if ((rojo < tono.R + tol && rojo > tono.R - tolerancia) && (verde < tono.G + tol && verde > tono.G - tolerancia) && (azul < tono.B + tol && azul > tono.B - tolerancia))
                    {
                        //si es de la tonalidad... el pixel se convierte al color de tono.
                        bmp_temp.SetPixel(i, j, tono);
                    }
                    else
                    {
                        //si no es de la tonalidad... el pixel se pinta de negro.
                        bmp_temp.SetPixel(i, j, Color.Black);
                    }
                }
            }
            return bmp_temp;
        }
        
        //busca puntos, a modo de muestreo, para generar las ecuaciones de la recta., importante manejar la tolerancia
        public Bitmap Ecuaciones(Bitmap bmp_temp)
        {
            //bases  contas
            double escalaA = 0.45;//de A (superior) a B(inferior), buscará los puntos de 4 dedos largos,  izquierda a derecha.
            double escalaB = 0.60;
          //  double escalaC = 0.5;//de C(SUP) A B(inf)... buscara los puntos del pulgar
            //double escalaD = 0.7;
            //baseY4, la base de
            baseA = (int)(bmp_temp.Height * escalaA);
            baseB = (int)(bmp_temp.Height * escalaB);
            //auxiliares.dff
            bool escandidato = false;
            bool encontrado = false;
            int ncandidatos;
            int pixel;
            int tolerancia = 3;
            int npixeles = 0;
            int componenteA, componenteB;
            int auxA, auxB, auxC, auxD, auxE, auxF, auxG, auxH;
            Color color_aux;
            //empezamos el recorrido
            for (int j = baseA; j < baseB; j++)
            {
                escandidato = false;
                ncandidatos = 0; componenteA = 0;
                auxA = 0; auxB = 0; auxC = 0; auxD = 0; auxE = 0; auxF = 0; auxG = 0; auxH = 0;
                for (int i = 0; i < bmp_temp.Width; i++) //de izquierda a derecha, y de arriba para abajoj.
                {
                    //linea por linea vamos recolectando píxeles todo en base anterior.
                    color_aux = new Color(bmp_temp.GetPixel(i, j));
                    if (color_aux.R == 0) //leemos la componente Rojo
                    {
                        pixel = 0; //es un pixel negro.
                    }
                    else
                    {
                        pixel = 1; // es un pixel que tiene tono.
                    }

                    if (escandidato == true)
                    {
                        if (pixel == 1)
                        {
                            npixeles += 1;
                            if (npixeles == tolerancia)
                            {
                                //si alcanzó la tolerancia..
                                encontrado = true;
                            }
                        }
                        else
                        {
                            npixeles = 0;
                            if (encontrado == false)
                            {
                                //no fue encontrado... 
                                escandidato = false; //desactivamos la búsqueda.
                                componenteA = 0; componenteB = 0; //borramos las componentes.
                            }
                            else
                            {
                                //si se encontró hasta aquí termina la componente B.
                                componenteB = i;
                                //aumentamos en uno el contador.
                                ncandidatos += 1;
                                switch (ncandidatos)
                                {
                                    case 1: auxA = componenteA; auxB = componenteB; break;
                                    case 2: auxC = componenteA; auxD = componenteB; break;
                                    case 3: auxE = componenteA; auxF = componenteB; break;
                                    case 4: auxG = componenteA; auxH = componenteB; break;
                                }
                                encontrado = false;//para que vuelva a buscar.
                            }
                        }


                    }
                    else
                    {
                        //no estamos buscando un candidato
                        if (pixel == 1)
                        {
                            escandidato = true; //empezamos a buscar un candidato si detecto un tono.
                            componenteA = i;
                        }//caso contrario no hacemos nada...
                    }

                }
                //si son 4 candidatos o màs.. tenemo los 4 dedos.
                if (ncandidatos >= 4)
                {
                    bmp_temp.SetPixel(0, 0, Color.Black);
                    //guardamos los puntos encontrados
                    componentes[nmuestras, 0] = new Point((int)(auxA + auxB) / 2, j);
                    componentes[nmuestras, 1] = new Point((int)(auxC + auxD) / 2, j);
                    componentes[nmuestras, 2] = new Point((int)(auxE + auxF) / 2, j);
                    componentes[nmuestras, 3] = new Point((int)(auxG + auxH) / 2, j);
                    //pintamos los puntos
                    bmp_temp.SetPixel(componentes[nmuestras, 0].X, j, Color.Black);
                    bmp_temp.SetPixel(componentes[nmuestras, 1].X, j, Color.DarkGreen);
                    bmp_temp.SetPixel(componentes[nmuestras, 2].X, j, Color.DarkCyan);
                    bmp_temp.SetPixel(componentes[nmuestras, 3].X, j, Color.DarkRed);
                    //aumentaos en uno el número de muestras
                    nmuestras++;//aumentamos en una las muestras
                }
                else {
                    bmp_temp.SetPixel(0, 0, Color.Fuchsia);
                    bmp_temp.SetPixel(0, baseA, Color.Fuchsia);
                    bmp_temp.SetPixel(0, baseB, Color.Fuchsia);
                }
            }
            return bmp_temp;
        }

        //busca los coeficientes A y B de las ecuaciones de la recta.
        public Bitmap Recta(Bitmap bmp_temp)
        {
            double x2 = 0;
            double y = 0;
            double x = 0;
            double xy = 0;
            double coef_a = 0;
            double coef_b = 0;
            int aux=0;
            for (int j = 0; j < 4; j++)
            {
                x2 = 0;
                y = 0;
                x = 0;
                xy = 0;
                coef_a = 0;
                coef_b = 0;
                for (int i = 0; i < nmuestras; i++) //de izquierda a derecha, y de arriba para abajoj.
                {
                    x2 += componentes[i, j].X * componentes[i, j].X;
                    y += componentes[i, j].Y;
                    x += componentes[i, j].X;
                    xy += componentes[i, j].X * componentes[i, j].Y;
                }
                //coeficiente a
                coef_b = (nmuestras * xy - x * y) / (nmuestras * x2 - x * x);
                coef_a = (y - coef_b * x) / nmuestras;
                //guardamos cada elemento en el array
                coeficiente_a[j] = coef_a;
                coeficiente_b[j] = coef_b;
                //parece que se debe eliminar
               // ecuacion1_a = coef_a; ecuacion1_b = coef_b;
                //calculamos los puntos de cada recta de muestra
                aux = (int)((baseA - coef_a) / coef_b);//x=(y-a)b
                recta_Ax[j] = aux;
                recta_Ay[j] = baseA;
                aux = (int)((baseB - coef_a) / coef_b);//x=(y-a)b
                recta_Bx[j] = aux; 
                recta_By[j] = baseB;
                bmp_temp.SetPixel(recta_Ax[j], recta_Ay[j], Color.White);
                bmp_temp.SetPixel(recta_Bx[j], recta_By[j], Color.White);
            }
            return bmp_temp;
        }
        //superponer dos imágenes
        public Bitmap Superponer(Bitmap bmp_tmp, Bitmap bmp_over, int xPos, int yPos) {
            Color color;
            int k,l;
            for (int i = xPos; i < xPos+ bmp_over.Width;i++ )
            {
                for (int j = yPos; j > yPos - bmp_over.Height; j--)
                {
                    //para poder ubicar el píxel un requisito es que el pixel exista.
                    if (i >= 0 && i<bmp_tmp.Width && j >= 0 && j < bmp_tmp.Height ) { 
                        //si ambos valores son positivos, entonces podremos ejecutar el código sin problemas.
                        //ahora analizamos si el valor es alfa
                        k = i - xPos;//coord x de la imagen over.
                        l = bmp_over.Height -1 - yPos + j;
                        if (k >= 0 && l >= 0)//tienen que ser positivos los valores.
                        {
                            color = new Color(bmp_over.GetPixel(k, l));
                            if(color.A == 255)
                            {
                                //consideramos porque... este color no tiene transparencia.
                                bmp_tmp.SetPixel(i, j, color);
                            }
                        }
                    }
                }
            }
            return bmp_tmp;
        }
        //recortar imagen
        public Bitmap Recortar(Bitmap bmp_temp, Double xx, Double yy, bool dimensiones)
        {
            int dx,dy; //nuevas dimensiones de la imagen
            if (dimensiones==true){
                //quiere decir que se están pasando las nuevas dimensiones, no hay que escalar.
                dx = (int)xx;
                dy = (int)yy;
            }
            else{
                //quiere decir que estamos pasando las proporciones y se requiere reescalar
                dx = (int)(bmp_temp.Width * xx);
                dy = (int)(bmp_temp.Height * yy);
            }
            bmp_temp = Bitmap.CreateScaledBitmap(bmp_temp, dx, dy, true);
            return bmp_temp;
        }
        //rotar imagen
        public Bitmap Rotar(Bitmap bmp_temp,int grados)
        {
            Matrix matrix = new Matrix();
            matrix.PostRotate(grados);
            Bitmap bmp_rotado = Bitmap.CreateBitmap(bmp_temp, 0, 0, bmp_temp.Width, bmp_temp.Height, matrix, true);
            return bmp_rotado;
        }
        
        //escalar dimensiones 
        public int Escalar(int valor, int dimension1, int dimension2)
        {
            int escalado = 0;
            float calculo = (float)dimension2 / (float)dimension1;
            calculo = calculo * (float)valor;
            escalado = (int)calculo;
            //System.Console.Out.WriteLine(" .... escalado: " + escalado + " calculo:"+ calculo);
            return escalado;
        }

        //calcuar puntos donde irán ubicadas las uñas y el tamaño de las uñas.
        public bool Unias()
        {
            bool error = true;
            //vamos a devolver
            return error;
        }
        //si no se encontraron las uñas, entonces procedemos a ubicar aproximadamente donde deben de estar
        public void Ejemplo(Bitmap bmp_piel)
        {
            //dado_interrar
            int base_width= 120;
            int base_height = 200;
            Point[] unia_ejemplo = new Point[5];
            int[] grosor_ejemplo = new int[5];
            int x_aux, y_aux;
            //posiciones de ejemplo
            unia_ejemplo[0]= new Point(16,105); grosor_ejemplo[0] = 8;
            unia_ejemplo[1] = new Point(30, 77); grosor_ejemplo[1] = 10;
            unia_ejemplo[2] = new Point(50, 68); grosor_ejemplo[2] = 11;
            unia_ejemplo[3] = new Point(70, 78); grosor_ejemplo[3] = 10;
            unia_ejemplo[4] = new Point(90, 130); grosor_ejemplo[4] = 7;
            for (int k = 0; k < 5; k++)
            {
                //escalamos según nuestra base y el lienzo recibido.
                x_aux = Escalar(unia_ejemplo[k].X, base_width, bmp_piel.Width);
                y_aux = Escalar(unia_ejemplo[k].Y, base_height, bmp_piel.Height);
                //  System.Console.Out.WriteLine("------------------------ y_aux:" + y_aux + " base_height:" + base_height + " bmp_piel.h:" + bmp_piel.Height + " ejemplo y:" + unia_ejemplo[k].Y);
                
                unias[k] = new Point(x_aux, y_aux);
                //escalamos y guardamos
                grosor[k] = Escalar(grosor_ejemplo[k], base_width, bmp_piel.Width);
            }
        }
        //********************************** construcción ************************/
        public Bitmap Construir()
        {
            Bitmap bmp_temp = bmp_original.Copy(Bitmap.Config.Rgb565, true);
            Color mitono = Tonalidad();
            Bitmap mipiel = Piel(mitono, 60, bmp_temp);
            //Bitmap mispuntos = mipiel.Copy(Bitmap.Config.Rgb565, true); 
            //mispuntos = Ecuaciones(mipiel);
            //Bitmap misrectas = mispuntos.Copy(Bitmap.Config.Rgb565, true);
            //misrectas = Recta(misrectas);
            Boolean error = Unias();//para pruebas va a devolver error.
            if (error == true) {
                Ejemplo(mipiel);
            }
            return mipiel;
        }
        //ubicamos las uñas..... necesitamos las 3 imágenes, 1: imagen.
        public Bitmap Ubicar(Bitmap bmp_final2,Bitmap bmp_algoritmo,Bitmap bmp_unia,Bitmap bmp_unia2)
        {
            //primerooooo 
            int uni_w,uni_h,pos_x,pos_y;//_w y _h, son las dimesiones escaladas de la unia, posX y posY sonn las posiciones de la unia.
            int[] rot = new int[5];//el último ya está rotado.
            Bitmap[] bmp_unias = new Bitmap[5];
            //definimmos los ángulos de rotación.
            rot[0] = -20; //meñique
            rot[1] = -10; //anular
            rot[2] = 0; //medio
            rot[3] = 10; //índice
            rot[4] = 45; // pulgar
            //hacemos una copia de las 5 uñas.... 
            bmp_unias[0] = bmp_unia.Copy(Bitmap.Config.Argb8888, true);
            bmp_unias[1] = bmp_unia.Copy(Bitmap.Config.Argb8888, true);
            bmp_unias[2] = bmp_unia.Copy(Bitmap.Config.Argb8888, true);
            bmp_unias[3] = bmp_unia.Copy(Bitmap.Config.Argb8888, true);
            bmp_unias[4] = bmp_unia.Copy(Bitmap.Config.Argb8888, true);
            //escalamos las proporciones para cada unia
            for (int k = 0; k < 5; k++)
            {
                //grosor para cada una de las uñas.
                uni_w = Escalar(grosor[k], bmp_algoritmo.Width, bmp_final2.Width);
                uni_h = Escalar(uni_w, bmp_unias[k].Width, bmp_unias[k].Height);
                //escalamos la uña a las dimensiones que requerimos. y rotar también.
                bmp_unias[k] = Recortar(bmp_unias[k], uni_w, uni_h, true);
                bmp_unias[k] = Rotar(bmp_unias[k], rot[k]);
                //calculamoss la posición de cada uña..
                pos_x = Escalar(unias[k].X, bmp_algoritmo.Width, bmp_final2.Width);
                pos_y = Escalar(unias[k].Y, bmp_algoritmo.Height, bmp_final2.Height);
                //superponemos
                //System.Console.Out.WriteLine("------------------------ x:" + pos_x + " y:" + unias[k].Y);
                bmp_final2= Superponer(bmp_final2, bmp_unias[k], pos_x, pos_y);
            }
            
            /*
            uni_w = Escalar(grosor[0], bmp_algoritmo.Width, bmp_final2.Width);
            uni_h = Escalar(uni_w, bmp_unia.Width, bmp_unia.Height);
            bmp_unia = Recortar(bmp_unia, uni_w, uni_h, true);
            bmp_unia = Rotar(bmp_unia, -20);
            pos_x = Escalar(unias[0].X, bmp_algoritmo.Width, bmp_final2.Width);
            pos_y = Escalar(unias[0].Y, bmp_algoritmo.Height, bmp_final2.Height);
            bmp_algoritmo = Superponer(bmp_final2, bmp_unia, pos_x, pos_y);
            */
            return bmp_final2;
        }
    }
}
