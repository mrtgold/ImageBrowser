//﻿//-------------------------------------------------------------------------- 
////  
////  Copyright (c) Microsoft Corporation.  All rights reserved.  
////  
////  File: PingExtensions.cs 
//// 
////-------------------------------------------------------------------------- 

//using System.ComponentModel;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using NUnit.Framework;

//namespace System.Net.NetworkInformation
//{
//    [TestFixture]
//    public class Test
//    {
//        [Test]
//        public void testThis()
//        {
//            var addrs = Dns.GetHostAddresses("www.google.com");
//            var pings = addrs.Select(addr => new Ping().SendTask(addr, null)).ToArray();

//            Task.Factory.ContinueWhenAll(pings, _ =>
//                                                    {
//                                                        Task.WaitAll(pings);
//                                                        foreach (var ping in pings)
//                                                        {
//                                                            Console.WriteLine("{0}: {1}", ping.Result.Status,
//                                                                              ping.Result.Address);
//                                                            Thread.Sleep(1000);
//                                                        }

//                                                    });

//            Console.WriteLine(@"See - main didn't wait!");

//            Thread.Sleep(5000);
//        }

//    }
//    /// <summary>Extension methods for working with Ping asynchronously.</summary> 
//    public static class PingExtensions
//    {
//        /// <summary> 
//        /// Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message. 
//        /// </summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="address">An IPAddress that identifies the computer that is the destination for the ICMP echo message.</param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <returns>A task that represents the asynchronous operation.</returns> 
//        public static Task<PingReply> SendTask(this Ping ping, IPAddress address, object userToken)
//        {
//            return SendTaskCore(ping, userToken, tcs => ping.SendAsync(address, tcs));
//        }

//        /// <summary> 
//        /// Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message. 
//        /// </summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="hostNameOrAddress"> 
//        /// A String that identifies the computer that is the destination for the ICMP echo message.  
//        /// The value specified for this parameter can be a host name or a string representation of an IP address. 
//        /// </param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <returns>A task that represents the asynchronous operation.</returns> 
//        public static Task<PingReply> SendTask(this Ping ping, string hostNameOrAddress, object userToken)
//        {
//            return SendTaskCore(ping, userToken, tcs => ping.SendAsync(hostNameOrAddress, tcs));
//        }

//        /// <summary> 
//        /// Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message. 
//        /// </summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="address">An IPAddress that identifies the computer that is the destination for the ICMP echo message.</param> 
//        /// <param name="timeout"> 
//        /// An Int32 value that specifies the maximum number of milliseconds (after sending the echo message)  
//        /// to wait for the ICMP echo reply message. 
//        /// </param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <returns>A task that represents the asynchronous operation.</returns> 
//        public static Task<PingReply> SendTask(this Ping ping, IPAddress address, int timeout, object userToken)
//        {
//            return SendTaskCore(ping, userToken, tcs => ping.SendAsync(address, timeout, tcs));
//        }

//        /// <summary> 
//        /// Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message. 
//        /// </summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="hostNameOrAddress"> 
//        /// A String that identifies the computer that is the destination for the ICMP echo message.  
//        /// The value specified for this parameter can be a host name or a string representation of an IP address. 
//        /// </param> 
//        /// <param name="timeout"> 
//        /// An Int32 value that specifies the maximum number of milliseconds (after sending the echo message)  
//        /// to wait for the ICMP echo reply message. 
//        /// </param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <returns>A task that represents the asynchronous operation.</returns> 
//        public static Task<PingReply> SendTask(this Ping ping, string hostNameOrAddress, int timeout, object userToken)
//        {
//            return SendTaskCore(ping, userToken, tcs => ping.SendAsync(hostNameOrAddress, timeout, tcs));
//        }

//        /// <summary> 
//        /// Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message. 
//        /// </summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="address">An IPAddress that identifies the computer that is the destination for the ICMP echo message.</param> 
//        /// <param name="timeout"> 
//        /// An Int32 value that specifies the maximum number of milliseconds (after sending the echo message)  
//        /// to wait for the ICMP echo reply message. 
//        /// </param> 
//        /// <param name="buffer"> 
//        /// A Byte array that contains data to be sent with the ICMP echo message and returned  
//        /// in the ICMP echo reply message. The array cannot contain more than 65,500 bytes. 
//        /// </param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <returns>A task that represents the asynchronous operation.</returns> 
//        public static Task<PingReply> SendTask(this Ping ping, IPAddress address, int timeout, byte[] buffer, object userToken)
//        {
//            return SendTaskCore(ping, userToken, tcs => ping.SendAsync(address, timeout, buffer, tcs));
//        }

//        /// <summary> 
//        /// Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message. 
//        /// </summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="hostNameOrAddress"> 
//        /// A String that identifies the computer that is the destination for the ICMP echo message.  
//        /// The value specified for this parameter can be a host name or a string representation of an IP address. 
//        /// </param> 
//        /// <param name="timeout"> 
//        /// An Int32 value that specifies the maximum number of milliseconds (after sending the echo message)  
//        /// to wait for the ICMP echo reply message. 
//        /// </param> 
//        /// <param name="buffer"> 
//        /// A Byte array that contains data to be sent with the ICMP echo message and returned  
//        /// in the ICMP echo reply message. The array cannot contain more than 65,500 bytes. 
//        /// </param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <returns>A task that represents the asynchronous operation.</returns> 
//        public static Task<PingReply> SendTask(this Ping ping, string hostNameOrAddress, int timeout, byte[] buffer, object userToken)
//        {
//            return SendTaskCore(ping, userToken, tcs => ping.SendAsync(hostNameOrAddress, timeout, buffer, tcs));
//        }

//        /// <summary> 
//        /// Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message. 
//        /// </summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="address">An IPAddress that identifies the computer that is the destination for the ICMP echo message.</param> 
//        /// <param name="timeout"> 
//        /// An Int32 value that specifies the maximum number of milliseconds (after sending the echo message)  
//        /// to wait for the ICMP echo reply message. 
//        /// </param> 
//        /// <param name="buffer"> 
//        /// A Byte array that contains data to be sent with the ICMP echo message and returned  
//        /// in the ICMP echo reply message. The array cannot contain more than 65,500 bytes. 
//        /// </param> 
//        /// <param name="options">A PingOptions object used to control fragmentation and Time-to-Live values for the ICMP echo message packet.</param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <returns>A task that represents the asynchronous operation.</returns> 
//        public static Task<PingReply> SendTask(this Ping ping, IPAddress address, int timeout, byte[] buffer, PingOptions options, object userToken)
//        {
//            return SendTaskCore(ping, userToken, tcs => ping.SendAsync(address, timeout, buffer, options, tcs));
//        }

//        /// <summary> 
//        /// Asynchronously attempts to send an Internet Control Message Protocol (ICMP) echo message. 
//        /// </summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="hostNameOrAddress"> 
//        /// A String that identifies the computer that is the destination for the ICMP echo message.  
//        /// The value specified for this parameter can be a host name or a string representation of an IP address. 
//        /// </param> 
//        /// <param name="timeout"> 
//        /// An Int32 value that specifies the maximum number of milliseconds (after sending the echo message)  
//        /// to wait for the ICMP echo reply message. 
//        /// </param> 
//        /// <param name="buffer"> 
//        /// A Byte array that contains data to be sent with the ICMP echo message and returned  
//        /// in the ICMP echo reply message. The array cannot contain more than 65,500 bytes. 
//        /// </param> 
//        /// <param name="options">A PingOptions object used to control fragmentation and Time-to-Live values for the ICMP echo message packet.</param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <returns>A task that represents the asynchronous operation.</returns> 
//        public static Task<PingReply> SendTask(this Ping ping, string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options, object userToken)
//        {
//            return SendTaskCore(ping, userToken, tcs => ping.SendAsync(hostNameOrAddress, timeout, buffer, options, tcs));
//        }

//        /// <summary>The core implementation of SendTask.</summary> 
//        /// <param name="ping">The Ping.</param> 
//        /// <param name="userToken">A user-defined object stored in the resulting Task.</param> 
//        /// <param name="sendAsync"> 
//        /// A delegate that initiates the asynchronous send. 
//        /// The provided TaskCompletionSource must be passed as the user-supplied state to the actual Ping.SendAsync method. 
//        /// </param> 
//        /// <returns></returns> 
//        private static Task<PingReply> SendTaskCore(Ping ping, object userToken, Action<TaskCompletionSource<PingReply>> sendAsync)
//        {
//            // Validate we're being used with a real smtpClient.  The rest of the arg validation 
//            // will happen in the call to sendAsync. 
//            if (ping == null) throw new ArgumentNullException("ping");

//            // Create a TaskCompletionSource to represent the operation 
//            var tcs = new TaskCompletionSource<PingReply>(userToken);

//            // Register a handler that will transfer completion results to the TCS Task 
//            PingCompletedEventHandler handler = null;
//            handler = (sender, e) => EAPCommon.HandleCompletion(tcs, e, () => e.Reply, () => ping.PingCompleted -= handler);
//            ping.PingCompleted += handler;

//            // Try to start the async operation.  If starting it fails (due to parameter validation) 
//            // unregister the handler before allowing the exception to propagate. 
//            try
//            {
//                sendAsync(tcs);
//            }
//            catch (Exception exc)
//            {
//                ping.PingCompleted -= handler;
//                tcs.TrySetException(exc);
//            }

//            // Return the task to represent the asynchronous operation 
//            return tcs.Task;
//        }
//    }

//    internal class EAPCommon
//    {
//        internal static void HandleCompletion<T>(
//            TaskCompletionSource<T> tcs, AsyncCompletedEventArgs e, Func<T> getResult, Action unregisterHandler)
//        {
//            // Transfers the results from the AsyncCompletedEventArgs and getResult() to the 
//            // TaskCompletionSource, but only AsyncCompletedEventArg's UserState matches the TCS 
//            // (this check is important if the same WebClient is used for multiple, asynchronous 
//            // operations concurrently).  Also unregisters the handler to avoid a leak. 
//            if (e.UserState == tcs)
//            {
//                if (e.Cancelled) tcs.TrySetCanceled();
//                else if (e.Error != null) tcs.TrySetException(e.Error);
//                else tcs.TrySetResult(getResult());
//                unregisterHandler();
//            }
//        }
//    } 

//}