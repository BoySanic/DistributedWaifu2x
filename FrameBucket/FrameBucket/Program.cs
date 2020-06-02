using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace FrameBucket
{
   
    class Program
    {

        public static string highrespath = @"C:\Users\npeterson\Desktop\FrameBucket\out";
        public static string lowResPath = @"C:\Users\npeterson\Desktop\FrameBucket\in";
        public static string projectFolder = @"C:\Users\npeterson\Desktop\FrameBucket";
        public static string lowResPathLocal = projectFolder + "\\start";
        public static string[] hostNames = { "pride", "ego", "death" };
        public static int numOfInstances = hostNames.Length;
        static void Main(string[] args)
        {
            
           /* string videoInputPath = args[0];
            StreamWriter sw = File.AppendText(projectFolder + "\\log.log");
            Console.WriteLine("Starting Server");
            string framesInPath = args[0];
            FFMpegAudioRip(videoInputPath, projectFolder);
            sw.Write("Framerip Start Time: " + DateTime.Now.ToLongTimeString() + "\n");
            sw.Flush();

            FFMpegFramesRip(videoInputPath, lowResPathLocal);
            sw.Write("Framerip End Time: " + DateTime.Now.ToLongTimeString() + "\n");
            sw.Flush();*/
            foreach (string s in hostNames)
            {
                if(!Directory.Exists(lowResPath + "\\" + s))
                {
                    Directory.CreateDirectory(lowResPath + "\\" + s);
                }
            }
            int frames = Directory.GetFiles(lowResPathLocal).Length;
            int index = 1;
            int remainingIndex = frames;
            //Move 10 frames into each bucket
            foreach (string s in hostNames)
            {
                index = MoveFilesToServer(lowResPath + "\\" + s + "\\", 10, lowResPathLocal, index);
            }
            
        }
        static int MoveFilesToServer(string serverPath, int numOfFrames, string lowrespath, int currIndex)
        {
            for (int i = 0; i < numOfFrames - 1; i++)
            {
                File.Move(lowResPathLocal + "\\frame" + currIndex.ToString("0000") + ".png", serverPath + "\\frame" + currIndex.ToString("0000") + ".png");
                currIndex += 1;
            }
            return currIndex;
        }
        static string[] CreateFoldersAndMoveFiles(string projectFolder, string lowrespath, int framesPerFolder, int ExtraFrames)
        {
            int[] frameLimits = new int[numOfInstances];
            int previousLastFrame = 0;
            string[] paths = new string[numOfInstances];
            for (int i = 0; i < numOfInstances; i++)
            {
                if (previousLastFrame == 0)
                {
                    frameLimits[i] = previousLastFrame + framesPerFolder + ExtraFrames;
                }
                else
                {
                    frameLimits[i] = previousLastFrame + framesPerFolder;
                }
                string splitPath = String.Format("{0}\\lowres{1}\\", projectFolder, i.ToString());
                string[] lowResFrames = Directory.GetFiles(lowrespath);
                if (!Directory.Exists(splitPath))
                {
                    Directory.CreateDirectory(splitPath);
                }
                Console.WriteLine("Folder #{0}: Frame {1} to Frame  {2}", i, previousLastFrame, frameLimits[i]);
                paths[i] = splitPath;
                for (int j = previousLastFrame + 1; j < frameLimits[i]; j++)
                {
                    string InFilePath = String.Format("{0}\\frame{1}.png", lowrespath, j.ToString("0000"));
                    string OutFilePath = String.Format("{0}\\frame{1}.png", splitPath, j.ToString("0000"));
                    File.Move(InFilePath, OutFilePath);

                }
                if (previousLastFrame == 0)
                    previousLastFrame += ExtraFrames;
                previousLastFrame += framesPerFolder;
            }
            return paths;
        }
        static void FFMpegAudioRip(string pathToVideoFile, string pathToOutputFolder)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("ffmpeg");
            p.StartInfo.Arguments = string.Format("-i {0} -vn -acodec copy {1}\\audio.m4a", pathToVideoFile, pathToOutputFolder);
            p.Start();
            p.WaitForExit();
        }
        static void FFMpegFramesRip(string pathToVideoFile, string pathToOutputFolder)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("ffmpeg");
            p.StartInfo.Arguments = string.Format("-i {0} {1}\\frame%04d.png", pathToVideoFile, pathToOutputFolder);
            p.Start();
            p.WaitForExit();
        }
    }
}
