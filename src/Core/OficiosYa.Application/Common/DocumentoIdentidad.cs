using System.Text.RegularExpressions;

namespace OficiosYa.Application.Common
{
    public static class DocumentoIdentidad
    {
        /// <summary>Deja solo dígitos.</summary>
        public static string Normalizar(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            return Regex.Replace(valor.Trim(), @"\D", "");
        }

        /// <summary>DNI 7-8 dígitos o CUIT/CUIL 11 dígitos.</summary>
        public static bool EsValido(string? valorNormalizado)
        {
            if (string.IsNullOrEmpty(valorNormalizado))
                return false;

            return valorNormalizado.Length is 7 or 8 or 11
                   && valorNormalizado.All(char.IsDigit);
        }

        public static string Etiqueta(string? valorNormalizado)
        {
            if (string.IsNullOrEmpty(valorNormalizado))
                return "DNI/CUIT";

            return valorNormalizado.Length == 11 ? "CUIT/CUIL" : "DNI";
        }
    }
}
