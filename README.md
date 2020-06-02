# DistributedWaifu2x
 Waifu2xVideoUpscaler is here for reference purposes. This was my first attempt at writing a program to do the waifu2x on a video thing

 This project intends to separate the jobs this program performed into several servers, and a client, so that the main workload can be heavily distributed

 FrameBucket is the server that deals with the client, and assigns work, and accepts work.

 FrameDigester will ingest footage, make a project from the resulting frames

 FrameShitter will monitor active projects and render a final video if a project completes all of its frames