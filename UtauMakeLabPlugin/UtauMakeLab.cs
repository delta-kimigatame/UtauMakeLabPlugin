using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using utauPlugin;
using System.Windows;
using System.Windows.Forms;

namespace UtauMakeLabPlugin
{
    class UtauMakeLab
    {
        static Regex eisuuReg = new Regex("[a-zA-Z0-9 _]*");
        static Regex kanjiReg = new Regex("[亜-熙]");
        static Regex vowelsA = new Regex("[ぁあかがさざただなはばぱまゃやらわァアカガサザタダナハバパマャヤラワ]$");
        static Regex vowelsI = new Regex("[ぃいきぎしじちぢにひびぴみりゐィイキギシジチヂニヒビピミリヰ]$");
        static Regex vowelsU = new Regex("[ぅうくぐすずつづぬふぶぷむゅゆるゥウクグスズツヅヌフブプムュユルヴ]$");
        static Regex vowelsE = new Regex("[ぇえけげせぜてでねへべぺめれゑェエケゲセゼテデネヘベペメレヱ]$");
        static Regex vowelsO = new Regex("[ぉおこごそぞとどのほぼぽもょよろをォオコゴソゾトドノホボポモョヨロヲ]$");
        static Regex vowelsN = new Regex("[んン]$");


        [STAThread]
        static void Main(string[] args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            int startTime = 0;
            int endTime = 0;
            String filePath = "test.lab";
            openFileDialog.Filter = "ラベルファイル(*.lab)|*.lab";
            openFileDialog.Title = "保存するファイル名";
            openFileDialog.CheckFileExists = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            else
            {
                Application.Exit();
            }


            UtauPlugin utauPlugin = new UtauPlugin(args[0]);
            utauPlugin.Input();
            StreamWriter writer = new StreamWriter(filePath,false);
            foreach(Note note in utauPlugin.note)
            {
                endTime = startTime + note.GetMsLength();
                writer.WriteLine(startTime.ToString() + "0000 "+ endTime.ToString()+"0000 "+GetVowel(note.GetLyric()));
                startTime = endTime;
            }
            writer.Close();
        }
        private static string GetVowel(String Lyric)
        {
            if (Lyric == "R")
            {
                return "sil";
            }
            Lyric = eisuuReg.Replace(Lyric, "");
            Lyric = kanjiReg.Replace(Lyric, "");
            if (vowelsA.IsMatch(Lyric))
            {
                return ("a");
            }else if (vowelsI.IsMatch(Lyric))
            {
                return ("i");
            }
            else if (vowelsU.IsMatch(Lyric))
            {
                return ("u");
            }
            else if (vowelsE.IsMatch(Lyric))
            {
                return ("e");
            }
            else if (vowelsO.IsMatch(Lyric))
            {
                return ("o");
            }
            else if (vowelsN.IsMatch(Lyric))
            {
                return ("N");
            }
            else
            {
                return "sil";
            }
        }
    }
}
