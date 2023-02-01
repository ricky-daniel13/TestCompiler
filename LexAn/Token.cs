using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexAn
{
    public enum TipoToken {PALCLAVE=0, LIT, PREPROC, IDEN, Max };
    public enum TipoLit { INTFLOAT = 0, HEX, BIN};
    public enum Tipos {INT = 0, BOOL, FLOAT, CHAR, FUNC, VOID, Max }

    
    public abstract class Token
    {
        public static string[] TipoVerb = { "Entero", "Booleano", "Flotante", "Caracter", "Funcion", "Vacio" , "Invalido" };

        public int from, to;
        public abstract TipoToken GetTipo();
        public abstract string GetInfo();
    }

    public sealed class TkIden : Token
    {
        public override TipoToken GetTipo() { return TipoToken.IDEN; }
        public string nombreIden;
        public TkIden(string nombreIden, int from, int to) { this.nombreIden = nombreIden; this.from = from; this.to = to; }
        public override string GetInfo() { return "Identificador de nombre: " + nombreIden; }
    }

    public sealed class TkClave : Token
    {
        public enum Claves {TIPO=0, SUMA, RESTA, MULT, DIV, MOD, AND, OR, BITAND, BITOR, NOT, 
            OPABRE, OPCIERR, BLABRE, BLCIERR, ACABRE, ACCIERR, COMP, ASIG, BITIZQ, BITDER, COMPMEN, COMPMAS, 
            RET, IF, ELSE, SAL, SIG, FOR, WHILE, PRINT, GET, FORMAT, COMA, CIERRE, MAX}

        public static string[] ClaveVerb =
            {"Tipo", "Suma", "Resta", "Multipliacion", "Division", "Modulo", "And", "Or", "And binario",
        "or Binario", "Negacion", "Apertura de funcion", "Cerradura de funcion", "Apertura de bloque",
        "Cerradura de bloque", "Apertura de acceso", "Cerradura de acceso", "Comparacion", "Asignacion",
        "Movimiento de bits izquierdo", "Movimiento de bits derecho", "Comparacion Menor que", "Comparacion Mayor que",
        "Comando de retorno", "Clausula If", "Clausula Else", "Comando de salida", "Comando de continuar",
        "Clausula de For", "Clausula de While", "Funcion de Impresion", "Funcion de obtencion", "Funcion de Formateo", "Coma", "Clausula de Cierre", "Invalido"};

        public override TipoToken GetTipo() { return TipoToken.PALCLAVE; }
        public Claves clave;
        public Tipos tipo;
        public TkClave(Claves clave, int from, int to) { this.clave = clave; this.from = from; this.to = to; }
        public TkClave(Tipos tipo, int from, int to) { this.clave = Claves.TIPO; this.tipo = tipo; this.from = from; this.to = to; }
        public override string GetInfo()
        {
            if (clave != Claves.TIPO)
                return ("Palabra clave: " + ClaveVerb[(int)clave]);
            else
                return ("Palabra clave tipo " + TipoVerb[(int)tipo]);
        }
    }

    public class TkLit : Token
    {
        public override TipoToken GetTipo() { return TipoToken.LIT; }
        public Tipos tipo;
        public bool isArray = false;
        public override string GetInfo() { return "Literal de tipo: " + TipoVerb[(int)tipo]; }
        public TkLit(Tipos tipo, int from, int to, bool isArray = false) { this.tipo = tipo; this.from = from; this.to = to; }
    }

    public class TkInv : Token
    {
        public override TipoToken GetTipo() { return TipoToken.Max; }
        public override string GetInfo() { return "Token Invalido."; }
        public TkInv(int from, int to) { this.from = from; this.to = to; }
    }

    public class TkPreProc : Token
    {
        public enum TipoPreProc {HEAPSIZE = 0, STACKSIZE, ENTRY }
        public static string[] TipoPreProcVerb = {"Tamaño de Monton", "Tamaño de Pila", "Punto de Entrada" };
        public override TipoToken GetTipo() { return TipoToken.PREPROC; }
        public Tipos tipo;
        public override string GetInfo() { return "Comando de preprocesador: " + TipoVerb[(int)tipo]; }
    }
}
