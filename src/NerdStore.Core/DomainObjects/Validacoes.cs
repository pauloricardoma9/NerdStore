using System.Text.RegularExpressions;

namespace NerdStore.Core.DomainObjects
{
    public class Validacoes
    {
        public static void ValidarSeIgual(object object1, object object2, string mensagem)
        {
            if (!object1.Equals(object2))
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeDiferente(object object1, object object2, string mensagem)
        {
            if (object1.Equals(object2))
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarExpressao(string pattern, string valor, string mensagem)
        {
            var regex = new Regex(pattern);

            if (!regex.IsMatch(valor))
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarTamanho(string valor, int maximo, string mensagem)
        {
            var length = valor.Trim().Length;
            if (length > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarTamanho(string valor, int minimo, int maximo, string mensagem)
        {
            var length = valor.Trim().Length;
            if (length < minimo || length > maximo)
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarSeNaoVazio(string valor, string mensagem)
        {
            if (valor == null || valor.Trim().Length == 0)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeNaoNulo(object object1, string mensagem)
        {
            if (object1 == null)
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarMinimoMaximo(double valor, double minimo, double maximo, string mensagem)
        {
            if (valor < minimo || valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarMinimoMaximo(float valor, float minimo, float maximo, string mensagem)
        {
            if (valor < minimo || valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarMinimoMaximo(int valor, int minimo, int maximo, string mensagem)
        {
            if (valor < minimo || valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarMinimoMaximo(long valor, long minimo, long maximo, string mensagem)
        {
            if (valor < minimo || valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarMinimoMaximo(decimal valor, decimal minimo, decimal maximo, string mensagem)
        {
            if (valor < minimo || valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarSeMenorQue(long valor, long maximo, string mensagem)
        {
            if (valor >= maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMenorQue(float valor, float maximo, string mensagem)
        {
            if (valor >= maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMenorQue(double valor, double maximo, string mensagem)
        {
            if (valor >= maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMenorQue(decimal valor, decimal maximo, string mensagem)
        {
            if (valor >= maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMenorQue(int valor, int maximo, string mensagem)
        {
            if (valor >= maximo)
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarSeMenorIgualQue(long valor, long maximo, string mensagem)
        {
            if (valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMenorIgualQue(float valor, float maximo, string mensagem)
        {
            if (valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMenorIgualQue(double valor, double maximo, string mensagem)
        {
            if (valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMenorIgualQue(decimal valor, decimal maximo, string mensagem)
        {
            if (valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMenorIgualQue(int valor, int maximo, string mensagem)
        {
            if (valor > maximo)
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarSeMaiorQue(long valor, long minimo, string mensagem)
        {
            if (valor <= minimo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMaiorQue(float valor, float minimo, string mensagem)
        {
            if (valor <= minimo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMaiorQue(double valor, double minimo, string mensagem)
        {
            if (valor <= minimo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMaiorQue(decimal valor, decimal minimo, string mensagem)
        {
            if (valor <= minimo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMaiorQue(int valor, int minimo, string mensagem)
        {
            if (valor <= minimo)
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarSeMaiorIgualQue(long valor, long minimo, string mensagem)
        {
            if (valor < minimo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMaiorIgualQue(float valor, float minimo, string mensagem)
        {
            if (valor < minimo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMaiorIgualQue(double valor, double minimo, string mensagem)
        {
            if (valor < minimo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMaiorIgualQue(decimal valor, decimal minimo, string mensagem)
        {
            if (valor < minimo)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeMaiorIgualQue(int valor, int minimo, string mensagem)
        {
            if (valor < minimo)
            {
                throw new DomainException(mensagem);
            }
        }

        public static void ValidarSeFalso(bool boolvalor, string mensagem)
        {
            if (boolvalor)
            {
                throw new DomainException(mensagem);
            }
        }
        public static void ValidarSeVerdadeiro(bool boolvalor, string mensagem)
        {
            if (!boolvalor)
            {
                throw new DomainException(mensagem);
            }
        }
    }
}