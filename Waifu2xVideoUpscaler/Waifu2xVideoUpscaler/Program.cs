using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Waifu2xVideoUpscaler
{
    class Program
    {
        static int numOfInstances = 0;
        static void Main(string[] args)
        {
            if(args.Length > 2)
                if (args[2].All(char.IsNumber))
                {
                    numOfInstances = Convert.ToInt32(args[2]);
                }
            
            int frames;
            int framesPerFolder;
            string lowResPath = args[1] + "\\lowres";
            string highResPath = args[1] + "\\highres";
            string projectFolder = args[1];
            string videoInputPath = args[0];
            StreamWriter sw = File.AppendText(projectFolder + "\\log.log");
            sw.Write("\nProgram Start Time: " + DateTime.Now.ToLongTimeString() + "\n");
            sw.Flush();
            //Check that paths exist
            if (!File.Exists(videoInputPath) || !Directory.Exists(projectFolder))
            {
                Console.WriteLine("Path not a valid folder!");
            }
            else
            {
                FFMpegAudioRip(videoInputPath, projectFolder);
                //Check folder structure, see if lowres and highres folders exist
                if (!Directory.Exists(lowResPath))
                {
                    Directory.CreateDirectory(lowResPath);
                }
                if (!Directory.Exists(highResPath))
                {
                    Directory.CreateDirectory(highResPath);
                }
                sw.Write("Framerip Start Time: " + DateTime.Now.ToLongTimeString() + "\n");
                sw.Flush();

                FFMpegFramesRip(videoInputPath, lowResPath);
                sw.Write("Framerip End Time: " + DateTime.Now.ToLongTimeString() + "\n");
                sw.Flush();
                if(numOfInstances > 0)
                {
                    frames = Directory.GetFiles(lowResPath).Length;
                    framesPerFolder = Convert.ToInt32(Math.Floor(Convert.ToDecimal(frames / numOfInstances)));
                    Console.WriteLine("Total Frames: " + frames);
                    Console.WriteLine("Frames Per folder: " + framesPerFolder);
                    int extraFrames = frames - framesPerFolder * numOfInstances;
                    Console.WriteLine("Extra Frames: " + extraFrames);
                    string[] paths = CreateFoldersAndMoveFiles(projectFolder, lowResPath, framesPerFolder, extraFrames);
                    string currentpath;
                    sw.Write("Waifu Start Time: " + DateTime.Now.ToLongTimeString() + "\n");
                    sw.Flush();
                    Parallel.ForEach(paths, (currentpath) => {
                        Console.WriteLine("Spawning Thread");
                        WaifuProcess(currentpath, highResPath);
                    });
                    sw.Write("Waifu End Time: " + DateTime.Now.ToLongTimeString() + "\n");
                    sw.Flush();
                }
                else
                {
                    frames = Directory.GetFiles(lowResPath).Length;
                    sw.Write("Waifu Start Time: " + DateTime.Now.ToLongTimeString() + "\n");
                    sw.Flush();
                    WaifuProcess(lowResPath, highResPath);
                    sw.Write("Waifu End Time: " + DateTime.Now.ToLongTimeString() + "\n");
                    sw.Flush();
                }
                sw.Write("Framebind Start Time: " + DateTime.Now.ToLongTimeString() + "\n");
                sw.Flush();

                FFMpegFramesBind(highResPath, projectFolder);

                sw.Write("Framebind End Time: " + DateTime.Now.ToLongTimeString() + "\n");
                sw.Flush();

                sw.Write("Audiobind Start Time: " + DateTime.Now.ToLongTimeString() + "\n");
                sw.Flush();

                FFMpegAddAudio(projectFolder + "\\Output.mp4", projectFolder + "\\audio.m4a", projectFolder);
                sw.Write("Audiobind End Time: " + DateTime.Now.ToLongTimeString() + "\n");
                sw.Flush();
                sw.Write("Program End Time: " + DateTime.Now.ToLongTimeString() + "\n");
                sw.Flush();
                /*frames = Directory.GetFiles(lowResPath).Length;
                framesPerFolder = Convert.ToInt32(Math.Floor(Convert.ToDecimal(frames / numOfInstances)));
                Console.WriteLine("Total Frames: " + frames);
                Console.WriteLine("Frames Per folder: " + framesPerFolder);
                int extraFrames = frames - framesPerFolder*numOfInstances;
                Console.WriteLine("Extra Frames: " + extraFrames);
                string[] paths = CreateFoldersAndMoveFiles(projectFolder, lowResPath, framesPerFolder, extraFrames);
                string currentpath;
                Parallel.ForEach(paths, (currentpath) => {
                    Console.WriteLine("Spawning Thread");
                    WaifuProcess(currentpath, highResPath);
                });*/
            }

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
        static void FFMpegFramesBind(string pathToImagesFolder, string pathToOutputFolder)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("ffmpeg");
            p.StartInfo.Arguments = string.Format("-r 29.97 -f image2 -s 1280x960 -i {0}\\frame%04d.png -vcodec libx264 -crf 25 -pix_fmt yuv420p -b:v 6000 {1}\\Output.mp4", pathToImagesFolder, pathToOutputFolder);
            p.Start();
            p.WaitForExit();
        }
        static void FFMpegAddAudio(string pathToVideoFile, string pathToAudioFile, string pathToOutputFolder)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("ffmpeg");
            p.StartInfo.Arguments = string.Format("-i {0} -i {1} -c copy -map 0:v:0 -map 1:a:0 {2}\\OutputEnd.mp4 -y", pathToVideoFile, pathToAudioFile, pathToOutputFolder);
            p.Start();
            p.WaitForExit();
        }
        static string[] CreateFoldersAndMoveFiles(string projectFolder, string lowrespath, int framesPerFolder, int ExtraFrames)
        {
            int[] frameLimits = new int[numOfInstances];
            int previousLastFrame = 0;
            string[] paths = new string[numOfInstances];
            for(int i = 0; i < numOfInstances; i++)
            {
                if(previousLastFrame == 0)
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
                if(previousLastFrame == 0)
                    previousLastFrame += ExtraFrames;
                previousLastFrame += framesPerFolder;
            }
            return paths;
        }

        static void WaifuProcess(string inPath, string outPath)
        {
            Directory.SetCurrentDirectory("C:\\waifu2x-converter\\");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("waifu2x-converter-cpp.exe");
            p.StartInfo.Arguments = string.Format("--scale-ratio 2.0 -a 0 -p 0 --noise-level 3 -i {0} -o {1}", inPath, outPath);
            p.Start();
            p.WaitForExit();
        }
        static void WaifuDenoiseProcess(string inPath, string outPath)
        {

        }
    }

}
