//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Threading.Tasks;
//using System.IO.Pipelines;
//using System.Buffers;
//using System.Security.Cryptography;

//namespace Vnr.Storage.API.TestFeatures.PipeTest
//{
//    public class TestPipe
//    {
//        public async Task ProcessLinesAsync(CryptoStream crStream)
//        {
//            var pipe = new Pipe();
//            Task writing = FillPipeAsync(crStream, pipe.Writer);
//            Task reading = ReadPipeAsync(pipe.Reader);

//            await Task.WhenAll(reading, writing);
//        }

//        private async Task FillPipeAsync(Socket socket, PipeWriter writer)
//        {
//            const int minimumBufferSize = 512;

//            while (true)
//            {
//                // Allocate at least 512 bytes from the PipeWriter.
//                Memory<byte> memory = writer.GetMemory(minimumBufferSize);
//                try
//                {
//                    int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
//                    if (bytesRead == 0)
//                    {
//                        break;
//                    }
//                    // Tell the PipeWriter how much was read from the Socket.
//                    writer.Advance(bytesRead);
//                }
//                catch (Exception ex)
//                {
//                    System.Console.WriteLine(ex);
//                    break;
//                }

//                // Make the data available to the PipeReader.
//                FlushResult result = await writer.FlushAsync();

//                if (result.IsCompleted)
//                {
//                    break;
//                }
//            }

//            // By completing PipeWriter, tell the PipeReader that there's no more data coming.
//            await writer.CompleteAsync();
//        }

//        private async Task ReadPipeAsync(PipeReader reader)
//        {
//            while (true)
//            {
//                ReadResult result = await reader.ReadAsync();
//                ReadOnlySequence<byte> buffer = result.Buffer;

//                while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
//                {
//                    // Process the line.
//                    ProcessLine(line);
//                }

//                // Tell the PipeReader how much of the buffer has been consumed.
//                reader.AdvanceTo(buffer.Start, buffer.End);

//                // Stop reading if there's no more data coming.
//                if (result.IsCompleted)
//                {
//                    break;
//                }
//            }

//            // Mark the PipeReader as complete.
//            await reader.CompleteAsync();
//        }

//        private bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
//        {
//            // Look for a EOL in the buffer.
//            SequencePosition? position = buffer.PositionOf((byte)'\n');

//            if (position == null)
//            {
//                line = default;
//                return false;
//            }

//            // Skip the line + the \n.
//            line = buffer.Slice(0, position.Value);
//            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
//            return true;
//        }
//    }
//}