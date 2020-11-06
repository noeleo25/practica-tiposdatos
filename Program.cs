using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticaTiposDatos
{
    class Program
    {
        //Ej 1 Enums
        public enum Categoria
        {
            Frontend, //0
            Backend,
            Movil, 
            Devops,
            ML
        }
        //Ej 2 Enums
        public enum SubCat:byte
        {
            Vue = 1,
            React = 2,
            Csharp = 11,
            Java = 12,
            Python = 13,
            Android = 21,
            IOs = 22,
        }
        //Ej 4 Enums

        [Flags]
        public enum Busqueda
        {
            Ninguno = 1,
            Categoria = 2,
            Subcategoria = 3,
            Reciente = 4,
            Todos = 5,
        }
        //IEnumerable: Union Parte 1 - poner antes
        public class Curso
        {
            public string Titulo { get; set; }
            public Categoria Categoria { get; set; }
            public SubCat SubCategoria { get; set; }
            public int Capitulos { get; set; }
        }
        public class Capitulo
        {
            public string Id { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public Curso Curso { get; set; }
        }
        static void Main()
        {
            #region Identificador literal @
            //Ej 1 Marca error 
            //string rutaArchivo = "C:\Users\noemi\source\repos\myfile.txt";
            //Ej 2
            string rutaArchivo = @"C:\Users\noemi\source\repos\myfile.txt";
            //Ej 3 Cadena de conexion
            string sqlServer = @"SERVER01\SQL";

            //Ej 4
            //string if = "if(a < b)"; marca error
            string @if = "if(a < b)\n{ \n ..\n}";
            Console.WriteLine($"Sentencia If:\n{@if}");
            #endregion

            #region Enum: usos,trucos y flags
            //Ej 3 Enums
            var enumvals = Enum.GetValues(typeof(Categoria));
            Console.WriteLine("Bienvenido.\nElige una categoría:");
            foreach (var ev in enumvals)
            {
                Console.WriteLine($"{(int)ev} - {ev}");
            }
            string cat = Console.ReadLine();
            Console.WriteLine($"Respuesta: {Enum.Parse(typeof(Categoria),cat)}");

            //Ej 4 - 1
            var val = Busqueda.Categoria | Busqueda.Reciente;
            Console.WriteLine(val.ToString());

            //Ej 4 - 2
            if (val.HasFlag(Busqueda.Ninguno))
                Console.WriteLine("Se incluye: Ninguno");
            if (val.HasFlag(Busqueda.Categoria))
                Console.WriteLine("Se incluye: Categoría");
            if (val.HasFlag(Busqueda.Subcategoria))
                Console.WriteLine("Se incluye: Subcategoría");
            if (val.HasFlag(Busqueda.Reciente))
                Console.WriteLine("Se incluye: Reciente");
            #endregion

            #region IEnumerables yield
            //Ej 1 - 2
            var imprime = EnumNombresOpcs(enumvals);

            /** Ej 3**/
            var enumvalsBus = Enum.GetValues(typeof(Busqueda));
            var imprimeB = EnumNombresOpcs(enumvalsBus);
            /** ./ Ej 3**/

            foreach (string s in imprime)
            {
                Console.Write(s);
            }
            #endregion

            #region IEnumerables Merge
            //Concat
            IEnumerable<string> todasOpcs = imprime.Concat(imprimeB);
            Console.WriteLine("Concat");
            foreach (string s in todasOpcs)
            {
                Console.Write(s);
            }
            //Join
            //Parte 2 - poner antes las clases
            Curso cursoA = new Curso 
            {   
                Titulo = "C# Trucos",
                Categoria = Categoria.Backend,
                SubCategoria = SubCat.Csharp,
                Capitulos = 5
            };
            Capitulo capituloA = new Capitulo
            {
                Id = "capA00",
                Titulo = "Capitulo 1 C#",
                Descripcion = "lorem ipsum ..",
                Curso = cursoA
            };
            Capitulo capituloB = new Capitulo
            {
                Id = "capB00",
                Titulo = "Capitulo 2 C#",
                Descripcion = "dolor sit amet ..",
                Curso = cursoA
            };
            List<Curso> cursos = new List<Curso> { cursoA };
            List<Capitulo> caps = new List<Capitulo> { capituloA, capituloB };
            //Join
            var union = cursos.Join(caps,
                                curso => curso,
                                cap => cap.Curso,
                                (curso, cap) => new { CursoTitulo = curso.Titulo, CapTitulo = cap.Titulo, CapDesc = cap.Descripcion }
                                );
            Console.WriteLine("--------Join--------");
            foreach (var el in union)
            {
                
                Console.WriteLine(
                    "{0} -> {1} : {2}",
                    el.CursoTitulo,
                    el.CapTitulo,
                    el.CapDesc);
            }

            //solo imprimir: para imprimir tambien podemos hacerlo asi de simple,
            Console.WriteLine("--------Capitulos--------");
            foreach (var cap in caps)
            {
                Console.WriteLine(
                    "{0} -> {1} : {2}",
                    cap.Curso.Titulo,
                    cap.Titulo,
                    cap.Descripcion);
            }
            #endregion

            #region Dictionary
            //Ej 1-1
            var duracionCapitulos = new Dictionary<string, TimeSpan>
            {
                { "capB02", new TimeSpan(05,25,36) },
                { "capA01", new TimeSpan(29,48,22) },
                { "capB01", new TimeSpan(10,57,31) }
            };
            //Ej 1-2
            var dKs = duracionCapitulos.Keys;
            var dVals = duracionCapitulos.Values;
            //Ej 1-3
            var contieneK = duracionCapitulos.ContainsKey(capituloA.Id);
            //Ej 1-4
            var ordenado = duracionCapitulos.OrderBy(dc => dc.Key);
            //Ej 1-5
            var codigosFirebaseAuthExcepcion = new Dictionary<string, string>
            {
                { "auth/email-already-exists", "El correo ya existe" },
                { "auth/id-token-expired", "El token ha expirado" },
                { "auth/internal-error", "Error interno" },
                { "auth/invalid-argument", "Se recibe un argumento inválido" },
                { "auth/invalid-email", "Correo electrónico inválido" }
            };
            string errorRecibido = "auth/invalid-email";
            Console.WriteLine($"Error de autenticación:\n{codigosFirebaseAuthExcepcion.GetValueOrDefault(errorRecibido)}");
            #endregion
        }
        //Ej 1-1 - IEnumerables YIELD 
        public static IEnumerable<string> EnumNombresOpcs(Array enumVals)
        {
            foreach (var ev in enumVals)
            {
                string opc = $"{(int)ev} - {ev}\n";
                yield return opc;
            }
        }
    }
}
