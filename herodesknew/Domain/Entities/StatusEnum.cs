using System.ComponentModel;

namespace herodesknew.Domain.Entities
{
    public enum StatusEnum
    {
        [Description("Aceita")]
        AC,

        [Description("Cancelado")]
        CA,

        //[Description("Controle de Mudança Detalhado")]
        //CD,

        //[Description("Concluído Remoto")]
        //CR,

        //[Description("Controle de Mudança Simplificado")]
        //CS,

        [Description("Concluído – Enviar Deploy")]
        DP,

        [Description("Em andamento")]
        EA,

        //[Description("Encaminhada")]
        //EN,

        //[Description("Homologação")]
        //HM,

        [Description("Concluído")]
        OK,

        //[Description("Pendente")]
        //PD,

        //[Description("Pronto para Homologação")]
        //PH,

        //[Description("Pronto para Produção")]
        //PP,

        //[Description("Produção")]
        //PR,

        //[Description("Recusada")]
        //RC,

        //[Description("Relatório do Controle de Log")]
        //RL,

        //[Description("Relatório de Validação")]
        //RV
    }

    public static class StatusEnumExtensions
    {
        public static StatusEnum ParseStatus(this string statusString)
        {
            if (Enum.TryParse(statusString, out StatusEnum status))
            {
                return status;
            }

            // You may handle the case when the input string doesn't match any enum value.
            // For simplicity, we'll return a default value or throw an exception.
            throw new ArgumentException("Invalid status string.", nameof(statusString));
        }
    }
}

