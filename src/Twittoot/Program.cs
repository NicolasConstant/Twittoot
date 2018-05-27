﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twittoot.Domain;
using Twittoot.Logic;

namespace Twittoot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var intro = new IntroDisplay();
                intro.Run();

                var logic = new TwittootLogic(new TwittootService());
                logic.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
