using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LexAn
{
    public class Lexer
    {
        string iO;
        int currPos;

        public Lexer(string iO)
        {
            this.iO = iO;
            currPos = 0;
        }
        public Token[] LexerAna(out int[] invToksIdx)
        {
            List<Token> tks = new List<Token>();
            List<int> invTks = new List<int>();

            Token currTk = null;
            do
            {
                currTk = PeekLex(out int tokenEnd);
                currPos = tokenEnd;
                //Console.WriteLine("--\nLeft code:\n" + iO.Substring(tokenEnd) + "\n\n\n--");
                if (currTk != null)
                {
                    tks.Add(currTk);
                    if (currTk.GetTipo() == TipoToken.Max)
                        invTks.Add(tks.Count - 1);
                }
            } while (currTk != null);

            invToksIdx = invTks.ToArray();

            return tks.ToArray();
        }

        bool FindMatch(string reg, out int startIdx, out int endIdx)
        {
            return FindMatch(reg, out startIdx, out endIdx, out _);
        }

        bool FindMatch(string reg, out int startIdx, out int endIdx, out string capture)
        {
            startIdx = -1;
            endIdx = -1;
            capture = "";
            //Console.WriteLine("Peek match: {{" + iO.Substring(currPos, 10) + "}}");
            Regex regEx = new Regex(reg);
            Match mtc = regEx.Match(iO, currPos);
            if (!mtc.Success)
                return false;
            //Console.WriteLine("Aproved!");
            startIdx = mtc.Index;
            endIdx = mtc.Index + mtc.Length;
            capture = mtc.Value;
            return true;
        }

        Token PeekLex(out int tokenEnd)
        {
            int ogPos = currPos;
            SkipWhiteSpace();
            int posAfterWh = currPos;
            //Console.WriteLine("--\nCode left after whitespace:\n" + iO.Substring(posAfterWh));
            Token tk = null;
            tk = LexClave();

            if (tk == null)
            {
                currPos = posAfterWh;
                tk = GetLit();
            }

            if (tk == null)
            {
                currPos = posAfterWh;
                tk = GetIden();
            }

            if(tk == null)
            {
                currPos = posAfterWh;
                tk = GetInv();
            }

            tokenEnd = currPos;
            currPos = ogPos;

            return tk;
        }

        TkClave LexClave()
        {
            int startIdx, endIdx;

            //----Check tipos----
            //Check int
            if(FindMatch("\\G\\bint\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(Tipos.INT, startIdx, endIdx);
            }
            //Check float
            if (FindMatch("\\G\\bbool\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(Tipos.BOOL, startIdx, endIdx);
            }
            if (FindMatch("\\G\\bfloat\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(Tipos.FLOAT, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bchar\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(Tipos.CHAR, startIdx, endIdx);
            }
            // SUMA
            if (FindMatch("\\G\\+", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.SUMA, startIdx, endIdx);
            }

            if (FindMatch("\\G\\-", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.RESTA, startIdx, endIdx);
            }

            if (FindMatch("\\G\\*", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.MULT, startIdx, endIdx);
            }

            if (FindMatch("\\G\\/", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.DIV, startIdx, endIdx);
            }

            if (FindMatch("\\G%", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.MOD, startIdx, endIdx);
            }

            if (FindMatch("\\G&&", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.AND, startIdx, endIdx);
            }

            if (FindMatch("\\G\\|\\|", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.OR, startIdx, endIdx);
            }

            if (FindMatch("\\G&", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.BITAND, startIdx, endIdx);
            }

            if (FindMatch("\\G\\|", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.BITOR, startIdx, endIdx);
            }

            if (FindMatch("\\G\\!", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.NOT, startIdx, endIdx);
            }

            if (FindMatch("\\G\\(", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.OPABRE, startIdx, endIdx);
            }

            if (FindMatch("\\G\\)", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.OPCIERR, startIdx, endIdx);
            }

            if (FindMatch("\\G\\{", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.BLABRE, startIdx, endIdx);
            }

            if (FindMatch("\\G\\}", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.BLCIERR, startIdx, endIdx);
            }

            if (FindMatch("\\G\\[", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.ACABRE, startIdx, endIdx);
            }

            if (FindMatch("\\G\\]", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.ACCIERR, startIdx, endIdx);
            }

            if (FindMatch("\\G==", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.COMP, startIdx, endIdx);
            }

            if (FindMatch("\\G=", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.ASIG, startIdx, endIdx);
            }
            if (FindMatch("\\G<<", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.BITIZQ, startIdx, endIdx);
            }

            if (FindMatch("\\G>>", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.BITDER, startIdx, endIdx);
            }

            if (FindMatch("\\G<", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.COMPMEN, startIdx, endIdx);
            }

            if (FindMatch("\\G>", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.COMPMAS, startIdx, endIdx);
            }

            if (FindMatch("\\G\\breturn\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.RET, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bif\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.IF, startIdx, endIdx);
            }

            if (FindMatch("\\G\\belse\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.ELSE, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bbreak\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.SAL, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bcontinue\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.SIG, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bfor\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.FOR, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bwhile\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.WHILE, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bprint\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.PRINT, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bget\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.GET, startIdx, endIdx);
            }

            if (FindMatch("\\G\\bformat\\b", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.FORMAT, startIdx, endIdx);
            }

            if (FindMatch("\\G,", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.COMA, startIdx, endIdx);
            }

            if (FindMatch("\\G;", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkClave(TkClave.Claves.CIERRE, startIdx, endIdx);
            }

            return null;
        }

        TkLit GetLit()
        {
            int startIdx, endIdx;

            //----Check tipos----
            //Check int
            if (FindMatch("\\G'[^']{1}'", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkLit(Tipos.CHAR, startIdx, endIdx);
            }

            if (FindMatch("\\G\"([^\"\\\\]*(\\\\.[^\"\\\\]*)*)", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkLit(Tipos.CHAR, startIdx, endIdx, true);
            }

            if (FindMatch("\\G(?!\\d*\\.)\\d+(?!\\.)", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkLit(Tipos.INT, startIdx, endIdx);
            }

            if (FindMatch("\\G\\d+(\\.\\d+)?", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return new TkLit(Tipos.FLOAT, startIdx, endIdx);
            }
            

            return null;
        }

        void SkipWhiteSpace()
        {
            int startIdx, endIdx;

            //----Check tipos----
            //Check int
            if (FindMatch("\\G\\s+", out startIdx, out endIdx))
            {
                currPos = endIdx;
                return;
            }

        }

        TkIden GetIden()
        {
            int startIdx, endIdx;

            //----Check tipos----
            //Check int
            if (FindMatch("\\G\\b[a-zA-Z][a-zA-Z0-9]*?\\b", out startIdx, out endIdx, out string cap))
            {
                currPos = endIdx;
                return new TkIden(cap, startIdx, endIdx);
            }

            return null;
        }

        TkInv GetInv()
        {
            int startIdx, endIdx;

            //----Check tipos----
            //Check int
            if (FindMatch("\\G[^\\s]*\\s", out startIdx, out endIdx, out string cap))
            {
                currPos = endIdx;
                return new TkInv(startIdx, endIdx);
            }

            return null;
        }
    }


}
