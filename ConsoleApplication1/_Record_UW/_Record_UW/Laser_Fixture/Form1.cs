using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;

namespace Laser_Fixture
{
    public partial class Form1 : Form
    {
        private bool bPLCThread = false;
        private int iActualValue = 0;
        private int iSettingValue = 0;
        private int iResultOld=(255);
        private int iOpen = 0;
        private int iClose = 0;
        private int iAbsolute = 0;
        private int iControl = 0;
        private int iCondition = 0;
        private int iTwoParts = 0;
        private int iActionMask = 0;
        private int iActionMask_Parts = 0;
        private int iNewAF = 0;
        private int ig_Count = 0;
        private string strFileName = "";
        private string strTime = "";
        private string connStr = "Provider=SQLOLEDB.1;Password=mesuser;Persist Security Info=True;User ID=mesuser;Initial Catalog=LASERDB;Data Source=172.28.11.34";
        private string StrMachineNo = "A288";
        private bool bNewAF = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Text = (0x2104 ^ 0x0100).ToString("X");

            lblPLC_Click(null, null);
        }
        void translateColor()
        {
            
        }
        void translatePLC(byte[] bt)
        {
            
            strTime = DateTime.Now.ToString("HHmmss-fff");
            string str =strTime+ ",";
            int iPos = 0;
            for (int j=0; j < 12; j++)
            {
                //1x
                for (int i = 0; i < 40; i++)
                {
                    str += (Convert.ToDouble(getUInt32(iPos, bt)) / 1000).ToString() + ",";
                    iPos += 4;
                }
            }
            //to 1920
           // MessageBox.Show(str);
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 50; i++)
                {
                    str += (bt[iPos]).ToString() + ",";
                    iPos += 1;
                }
            }
            //to 2020
           // MessageBox.Show(str);
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 50; i++)
                {
                    str += (Convert.ToDouble(getInt16(iPos,bt))/100).ToString() + ",";
                    iPos += 2;
                }
            }
            //to 2420
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 30; i++)
                {
                    str += (getFloat(iPos, bt)).ToString() + ",";
                    iPos += 4;
                }
                for (int i = 0; i < 50; i++)
                {
                    str += (getFloat(iPos, bt)).ToString() + ",";
                    iPos += 4;
                }
            }
            //to 3060
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 30; i++)
                {
                    str += (getInt16(iPos, bt)).ToString() + ",";
                    iPos += 2;
                }
                for (int i = 0; i < 50; i++)
                {
                    str += (getInt16(iPos, bt)).ToString() + ",";
                    iPos += 2;
                }
            }
            //to 3380
            strFileName = DateTime.Now.ToString("yyyyMMdd"); ;// DateTime.Now.ToString("yyyyMMdd-HHmmss-fff");
            saveCSVLog(strFileName + "_A", str);
            ReadProdave_UW();
            //MessageBox.Show(iPos.ToString());
            //MessageBox.Show(str);
        }
        private void saveCSVLog(string fname, string str)
        {
            string filePath = Application.StartupPath;
            //   bool bFileExist = (File.Exists(filePath));
            if (Directory.Exists("Log\\") == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory("Log\\");
            }
            fname = "Log\\" + fname + ".csv";
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(fname, System.IO.FileMode.Append);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.Default);
                //    if (!bFileExist) sw.WriteLine("");
               // MessageBox.Show(fname.Substring(fname.Length - 5, 1));
              //  MessageBox.Show(fs.Length.ToString());
                if (fs.Length < 10)
                {
                    //MessageBox.Show(fname.Substring(fname.Length - 1, 1));
                    if (fname.Substring(fname.Length - 5, 1) == "A")
                    {
                        string strT = "时间,";
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "1X[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "1Y[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "1Z[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "2X[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "2Y[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "2Z[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "3X[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "3Y[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "3Z[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "4X[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "4Y[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            strT += "4Z[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "12焊接状态[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "34焊接状态[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "压力反馈1[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "压力反馈2[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "压力反馈3[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "压力反馈4[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            strT += "1轴角度[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "2轴角度[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            strT += "3轴角度[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "4轴角度[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            strT += "1轴振幅[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "2轴振幅[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            strT += "3轴振幅[" + (i + 1).ToString() + "],";
                        }
                        for (int i = 0; i < 50; i++)
                        {
                            strT += "4轴振幅[" + (i + 1).ToString() + "],";
                        }
                        sw.WriteLine(strT);
                    }
                    else
                    {
                        string strT = "时间,";
                        for (int j = 0; j < 15; j++)
                        {
                            for (int i = 0; i < 30; i++)
                            {
                                strT += "1轴发生器M"+(j+1).ToString()+"[" + (i + 1).ToString() + "],";
                            }
                        }
                        for (int j = 0; j < 15; j++)
                        {
                            for (int i = 0; i < 30; i++)
                            {
                                strT += "2轴发生器M" + (j + 1).ToString() + "[" + (i + 1).ToString() + "],";
                            }
                        }
                        for (int j = 0; j < 15; j++)
                        {
                            for (int i = 0; i < 30; i++)
                            {
                                strT += "3轴发生器M" + (j + 1).ToString() + "[" + (i + 1).ToString() + "],";
                            }
                        }
                        for (int j = 0; j < 15; j++)
                        {
                            for (int i = 0; i < 30; i++)
                            {
                                strT += "4轴发生器M" + (j + 1).ToString() + "[" + (i + 1).ToString() + "],";
                            }
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            for (int i = 0; i < 15; i++)
                            {
                                strT += (j + 1).ToString()+"轴频率" +  "[" + (i + 1).ToString() + "],";
                            }
                        }
                        sw.WriteLine(strT);
                    }

                }
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            catch
            {

            }
            finally
            {

            }
        }
        void translatePLCUW(byte[] bt)
        {
           // string str = "";
            string str = strTime + ",";
            int iPos = 0;
            for (int j = 0; j < 60; j++)
            {
                for (int i = 0; i < 30; i++)
                {
                    str += ((getInt16(iPos, bt))).ToString() + ",";
                    iPos += 2;
                }
            }
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 15; i++)
                {
                    str += ((getInt16(iPos, bt))).ToString() + ",";
                    iPos += 2;
                }
            }
            
            saveCSVLog(strFileName + "_B", str);
        }
        void ReadProdave()
        {
            //   return;
            //     return;
            try
            {
            byte oldByteFlag=255;
            //v0.0 trigger     V9.0 force
            //vb2 code
            //v6.0 ok   v7.0 NG
            //v8.0 heart
            this.Invoke((EventHandler)delegate
                      {
                          lblPLC.Text = "开始连接PLC" +DateTime.Now.ToString();
                      });
            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;
            byte[] allBytes=new byte[30000];
            int rack = 0;
            int slot = 1;
            byte[] ZeroBytes = new byte[255];
            int i, a = 0, j, res, b = 0, c = 0;
            float d = 0;
            int iTotalLen = 3381;
            int dbNum = 666;
            for (int ii = 0; ii < 254; ii++)
            {
                ZeroBytes[ii] = 0;
            }
            fds.rfd = libnodave.openSocket(102, "192.168.1.3");//192.168.1.63                172.29.238.86
            fds.wfd = fds.rfd;
            if (fds.rfd > 0)
            {
                di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed1500k);
                di.setTimeout(1000000);
                //    res=di.initAdapter();	// does nothing in ISO_TCP. But call it to keep your programs indpendent of protocols
                //	    if(res==0) {
                dc = new libnodave.daveConnection(di, 0, rack, slot);                //  MessageBox.Show(dc.connectPLC().ToString());
                //  libnodave.daveexecWriteRequest
                if (0 == dc.connectPLC())
                {
                  
                    byte[] bt = new byte[] { 1 };
                    byte[] btBC = new byte[32];
                    byte[] btBC2 = new byte[16];
                    this.Invoke((EventHandler)delegate
                      {
                          lblPLC.Text = "PLC连接成功" + DateTime.Now.ToString();
                          lblPLC.BackColor = Color.Lime;
                      });
                    bPLCThread = true;
                    while (bPLCThread)
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            lblPLC.Text = "reading" + DateTime.Now.ToString();
                            //lblPLC.BackColor = Color.Lime;
                        });
                        Thread.Sleep(200);
                        byte[] dcBytes = new byte[255];
                        int iPos = 0;

                        res = dc.readBytes(libnodave.daveDB, 171, 3380, 1, dcBytes);
                        if (res == 0)
                        {
                            ig_Count = 0;
                            bool ctn = false;
                             this.Invoke((EventHandler)delegate
                                {
                            this.Text = dcBytes[0].ToString();
                            if (((dcBytes[0] & 1) != 0 & ((oldByteFlag & 1) == 0)) | checkBox1.Checked)
                            {
                                checkBox1.Checked = false;
                                //  translatePLC(allBytes);
                            }
                            else
                            {
                                ctn = true;
                                oldByteFlag = dcBytes[0];
                                
                            }
                                
                            oldByteFlag = dcBytes[0];
                                });
                             oldByteFlag = dcBytes[0];
                             if (ctn) continue;
                        }

                        Stopwatch sw = new Stopwatch();
                        sw.Reset();
                        sw.Start();
                        while (iPos < iTotalLen)
                        {                            
                            #region Reading....
                            int iLen = 200;
                            if (iTotalLen - iPos < 200) { iLen = iTotalLen - iPos; }
                            Array.Copy(ZeroBytes, dcBytes, 200);
                            res = dc.readBytes(libnodave.daveDB, 171, iPos, iLen, dcBytes);
                            Array.Copy(dcBytes, 0, allBytes, iPos, iLen);
                            sw.Stop();
                            int iResult = 0;
                            if (res == 0)
                            {
                                this.Invoke((EventHandler)delegate
                                {
                                    lblPLC.Text = "read ok" + DateTime.Now.ToString() + "  " + dcBytes[4].ToString() + " " + sw.ElapsedMilliseconds.ToString()+ " "+iPos.ToString();
                                    //lblPLC.BackColor = Color.Lime;
                                   // textBox1.Text = lblPLC.Text + "\r\n" + textBox1.Text;
                                });
                                Thread.Sleep(2);
                                sw.Start();
                                #region
                                //ig_Count = 0;
                                //for (int ii = 0; ii < 30; ii++)
                                //{
                                //    btBC[ii] = (byte)dc.getU8At(0 + ii);
                                //}
                                //for (int ii = 0; ii < 16; ii++)
                                //{
                                //    btBC2[ii] = (byte)dc.getU8At(50 + ii);
                                //}
                                //iResult = (int)(dc.getU8At(100));
                                //int iCode = dc.getU16At(80);
                                //this.Invoke((EventHandler)delegate
                                //    {
                                //        // txtFixNum.Text = iFixNum.ToString();
                                //        lblCode.Text = iCode.ToString();
                                //        lbBC.Text = System.Text.Encoding.Default.GetString(btBC);
                                //        lbFix.Text = System.Text.Encoding.Default.GetString(btBC2);
                                //        if ((iResult & 1) == 0)
                                //        {
                                //            lblOK.BackColor = Color.LightGray;
                                //        }
                                //        else
                                //        {
                                //            if ((iResultOld & 1) == 0) SaveToMDB(lbBC.Text, lbFix.Text, lblCode.Text, 1);
                                //            lblOK.BackColor = Color.Lime;
                                //        }
                                //        if ((iResult & 2) == 0)
                                //        {
                                //            lblNG.BackColor = Color.LightGray;
                                //        }
                                //        else
                                //        {
                                //            if ((iResultOld & 2) == 0) SaveToMDB(lbBC.Text, lbFix.Text, lblCode.Text, 2);
                                //            lblNG.BackColor = Color.Lime;
                                //        }

                                //    });
                                //80=code
                                #endregion
                                iResultOld = iResult;
                            }
                            else
                            {
                                bPLCThread = false;
                                this.Invoke((EventHandler)delegate
                                {
                                    libnodave.closeSocket(fds.rfd);
                                    timer1.Enabled = true;
                                    Thread.Sleep(100);
                                    lblPLC.Text = "读取PLC失败" + DateTime.Now.ToString();
                                    lblPLC.BackColor = Color.Red;
                                });
                            }
                            #endregion
                            iPos = iPos + 200;
                        }

                        
                         this.Invoke((EventHandler)delegate
                                {
                                    //this.Text = allBytes[3380].ToString();
                                    //if (((allBytes[3380] & 1) != 0 & ((oldByteFlag & 1) == 0))| checkBox1.Checked)
                                    //{
                                    //    checkBox1.Checked = false;
                                    //    translatePLC(allBytes);
                                    //}
                                    //oldByteFlag = allBytes[3380];
                                    translatePLC(allBytes);
                                });
                    }

                }
                
                dc.disconnectPLC();
                //	    }	    
                //	    di.disconnectAdapter();	// does nothing in ISO_TCP. But call it to keep your programs indpendent of protocols
                libnodave.closeSocket(fds.rfd);
            
                #region
                //this.Invoke((EventHandler)delegate
                //{
                //    timerPLC.Enabled = true;
                //    Thread.Sleep(100);
                //    lblPLC.Text = "读取PLC失败" + DateTime.Now.ToString();
                //    lblPLC.BackColor = Color.Red;
                //});
                #endregion
            }
            else
            {
                this.Invoke((EventHandler)delegate
                {
                    libnodave.closeSocket(fds.rfd);
                    timer1.Enabled = true;
                    Thread.Sleep(100);
                    lblPLC.Text = "连接PLC失败" + DateTime.Now.ToString();
                    lblPLC.BackColor = Color.Red;
                });


                //MessageBox.Show("Couldn't open TCP connaction to ");
                //  return -1;

            }
            }
            catch
            {
            }
            #region deleted
            // MessageBox.Show("end");
            //  libnodave.daveexecWriteRequest

            //  int rtVal = 1;
            //  short ConNr = 1; // First connection；(0 ... 63)；(max. 64 connections).
            //  string AccessPoint = "S7ONLINE"; // Default access point——S7ONLINE                  
            //  Prodave6_CS.Prodave6.CON_TABLE_TYPE ConTable;// Connection table
            //  int ConTableLen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Prodave6_CS.Prodave6.CON_TABLE_TYPE));// Length of the connection table

            //  ConTable.Adr = new byte[] { 192,168,2, 1, 0, 0 };
            //  ConTable.AdrType = 2;// (Byte)(globalVars.PSetting[9].iMode + 1); // Type of address: MPI/PB (1), IP (2), MAC (3)
            ///*  if (ConTable.AdrType > 3)
            //  {
            //      ConTable.AdrType = 3;
            //      ConTable.SlotNr = 1; // 插槽号
            //  }
            //  else
            //  {
            //      ConTable.SlotNr = 2;
            //  }
            //  ConTable.SlotNr = 1;
            //   * */
            //  ConTable.SlotNr = 1;
            //  ConTable.RackNr = 0; // 机架号  
            //  try
            //  {
            //      rtVal = Prodave6.LoadConnection_ex6(ConNr, AccessPoint, ConTableLen, ref ConTable);
            //  }
            //  catch// Exception(Ex)
            //  {
            //      //      MessageBox.Show("");
            //  }
            //  if (rtVal != 0)
            //  {
            //     globalVars.bPLCThread = false;
            //      return;
            //  }
            //  //以下测试SetActiveConnection_ex6
            //  UInt16 UConNr = (UInt16)ConNr;
            //  rtVal = Prodave6.SetActiveConnection_ex6(UConNr);
            //  while (globalVars.bPLCThread)
            //  {
            //      Thread.Sleep(200);
            //      UInt16 BlkNr = 1;//data block号
            //      UInt32 pDatLen = 0;
            //      Prodave6.DatType DType = Prodave6.DatType.BYTE;//要读取的数据类型
            //      //    DType = Prodave6.DatType.WORD;//要读取的数据类型
            //      UInt16 StartNr = 0;//起始地址号
            //      UInt32 pAmount = 10;//需要读取类型的数量 0-9是写  10-19是读
            //      UInt32 BufLen = 10;//缓冲区长度（字节为单位）
            //      //参数：data block号、要写入的数据类型、起始地址号、需要写入类型的数量、缓冲区长度（字节为单位）、缓冲区
            //      byte[] pWriteBuffer = new byte[20];
            //      rtVal = Prodave6.field_read_ex6(Prodave6.FieldType.D, 1, 10, pAmount, BufLen,globalVars.AGLBuff, ref pDatLen);
            //      globalVars.bConnState = (rtVal == 0);
            //      if ((globalVars.AGLBuff[0] & 1) != 0 & (globalVars.AGLBuffOld[0] & 1) == 0)
            //      {
            //          GC.Collect();
            //          trigCam();
            //      }

            //      Buffer.BlockCopy(globalVars.AGLBuff, 0, globalVars.AGLBuffOld, 0, globalVars.AGLBuff.Length);
            //      btnTest.Text=(rtVal.ToString()+ " "+globalVars.AGLBuff[0].ToString());           
            //  }

            //  Prodave6.UnloadConnection_ex6(UConNr);
            #endregion

        }
        int getInt16(int iPos, byte[] buf)
        {
            if (buf.Length < iPos + 2) return 0;
            byte[] bts = new byte[2];
            bts[0] = buf[iPos + 1];
            bts[1] = buf[iPos];
           return BitConverter.ToInt16(bts, 0);
        }
        uint getUInt16(int iPos, byte[] buf)
        {
            if (buf.Length < iPos + 2) return 0;
            byte[] bts = new byte[2];
            bts[0] = buf[iPos + 1];
            bts[1] = buf[iPos];
            return BitConverter.ToUInt16(bts, 0);
        }
        int getInt32(int iPos, byte[] buf)
        {
            if (buf.Length < iPos + 4) return 0;
            byte[] bts = new byte[4];
            bts[0] = buf[iPos + 3];
            bts[1] = buf[iPos + 2];
            bts[2] = buf[iPos + 1];
            bts[3] = buf[iPos];
            return BitConverter.ToInt32(bts, 0);            
        }
        UInt32 getUInt32(int iPos, byte[] buf)
        {
            if (buf.Length < iPos + 4) return 0;
            byte[] bts = new byte[4];
            bts[0] = buf[iPos + 3];
            bts[1] = buf[iPos + 2];
            bts[2] = buf[iPos + 1];
            bts[3] = buf[iPos];
            return BitConverter.ToUInt32(bts, 0);
        }
        float getFloat(int iPos, byte[] buf)
        {
            if (buf.Length < iPos + 4) return 0;
            byte[] bts = new byte[4];
            bts[0] = buf[iPos + 3];
            bts[1] = buf[iPos + 2];
            bts[2] = buf[iPos + 1];
            bts[3] = buf[iPos];
            return BitConverter.ToSingle(bts, 0);
        }
        void ReadProdave_UW()
        {
            try
            {
                this.Invoke((EventHandler)delegate
                {
                    lblPLC.Text = "开始连接PLC" + DateTime.Now.ToString();
                });
                libnodave.daveOSserialType fds;
                libnodave.daveInterface di;
                libnodave.daveConnection dc;
                byte[] allBytes = new byte[30000];
                int rack = 0;
                int slot = 1;
                byte[] ZeroBytes = new byte[255];
                int i, a = 0, j, res, b = 0, c = 0;
                float d = 0;
                int iTotalLen = 3720;
                int dbNum = 666;
                for (int ii = 0; ii < 254; ii++)
                {
                    ZeroBytes[ii] = 0;
                }
                fds.rfd = libnodave.openSocket(102, "192.168.1.63");//192.168.1.63                172.29.238.86
                fds.wfd = fds.rfd;
                if (fds.rfd > 0)
                {
                    di = new libnodave.daveInterface(fds, "IF2", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed1500k);
                    di.setTimeout(1000000);
                    //    res=di.initAdapter();	// does nothing in ISO_TCP. But call it to keep your programs indpendent of protocols
                    //	    if(res==0) {
                    dc = new libnodave.daveConnection(di, 0, rack, slot);                //  MessageBox.Show(dc.connectPLC().ToString());
                    //  libnodave.daveexecWriteRequest
                    if (0 == dc.connectPLC())
                    {

                        byte[] bt = new byte[] { 1 };
                        byte[] btBC = new byte[32];
                        byte[] btBC2 = new byte[16];
                        this.Invoke((EventHandler)delegate
                        {
                            lblPLC.Text = "PLC连接成功" + DateTime.Now.ToString();
                            lblPLC.BackColor = Color.Lime;
                        });
                        bPLCThread = true;
                        if (bPLCThread)
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                lblPLC.Text = "reading" + DateTime.Now.ToString();
                                //lblPLC.BackColor = Color.Lime;
                            });
                            Thread.Sleep(20);
                            byte[] dcBytes = new byte[255];
                            int iPos = 0;
                            Stopwatch sw = new Stopwatch();
                            sw.Reset();
                            sw.Start();
                            while (iPos < iTotalLen)
                            {
                                #region Reading....
                                int iLen = 200;
                                if (iTotalLen - iPos < 200) { iLen = iTotalLen - iPos; }
                                Array.Copy(ZeroBytes, dcBytes, 200);
                                res = dc.readBytes(libnodave.daveDB, 76, iPos, iLen, dcBytes);
                                Array.Copy(dcBytes, 0, allBytes, iPos, iLen);
                                sw.Stop();
                                int iResult = 0;
                                if (res == 0)
                                {
                                    this.Invoke((EventHandler)delegate
                                    {
                                        lblPLC.Text = "read ok" + DateTime.Now.ToString() + "  " + dcBytes[4].ToString() + " " + sw.ElapsedMilliseconds.ToString() + " " + iPos.ToString();
                                        //lblPLC.BackColor = Color.Lime;
                                        //   textBox1.Text = lblPLC.Text + "\r\n" + textBox1.Text;
                                    });
                                    Thread.Sleep(2);
                                    sw.Start();
                                    #region
                                    //ig_Count = 0;
                                    //for (int ii = 0; ii < 30; ii++)
                                    //{
                                    //    btBC[ii] = (byte)dc.getU8At(0 + ii);
                                    //}
                                    //for (int ii = 0; ii < 16; ii++)
                                    //{
                                    //    btBC2[ii] = (byte)dc.getU8At(50 + ii);
                                    //}
                                    //iResult = (int)(dc.getU8At(100));
                                    //int iCode = dc.getU16At(80);
                                    //this.Invoke((EventHandler)delegate
                                    //    {
                                    //        // txtFixNum.Text = iFixNum.ToString();
                                    //        lblCode.Text = iCode.ToString();
                                    //        lbBC.Text = System.Text.Encoding.Default.GetString(btBC);
                                    //        lbFix.Text = System.Text.Encoding.Default.GetString(btBC2);
                                    //        if ((iResult & 1) == 0)
                                    //        {
                                    //            lblOK.BackColor = Color.LightGray;
                                    //        }
                                    //        else
                                    //        {
                                    //            if ((iResultOld & 1) == 0) SaveToMDB(lbBC.Text, lbFix.Text, lblCode.Text, 1);
                                    //            lblOK.BackColor = Color.Lime;
                                    //        }
                                    //        if ((iResult & 2) == 0)
                                    //        {
                                    //            lblNG.BackColor = Color.LightGray;
                                    //        }
                                    //        else
                                    //        {
                                    //            if ((iResultOld & 2) == 0) SaveToMDB(lbBC.Text, lbFix.Text, lblCode.Text, 2);
                                    //            lblNG.BackColor = Color.Lime;
                                    //        }

                                    //    });
                                    //80=code
                                    #endregion
                                    iResultOld = iResult;
                                }
                                else
                                {
                                    bPLCThread = false;
                                    this.Invoke((EventHandler)delegate
                                    {
                                        libnodave.closeSocket(fds.rfd);
                                        timer1.Enabled = true;
                                        Thread.Sleep(100);
                                        lblPLC.Text = "读取PLC失败" + DateTime.Now.ToString();
                                        lblPLC.BackColor = Color.Red;
                                    });
                                }
                                #endregion
                                iPos = iPos + 200;
                            }

                        }

                        this.Invoke((EventHandler)delegate
                        {
                            translatePLCUW(allBytes);
                            this.lblCode.Text = allBytes[3719].ToString();
                            //    lblPLC.Text = "读取PLC失败" + DateTime.Now.ToString();
                            //   lblPLC.BackColor = Color.Red;
                        });
                    }
                    dc.disconnectPLC();
                    //	    }	    
                    //	    di.disconnectAdapter();	// does nothing in ISO_TCP. But call it to keep your programs indpendent of protocols
                    libnodave.closeSocket(fds.rfd);
                    #region
                    //this.Invoke((EventHandler)delegate
                    //{
                    //    timerPLC.Enabled = true;
                    //    Thread.Sleep(100);
                    //    lblPLC.Text = "读取PLC失败" + DateTime.Now.ToString();
                    //    lblPLC.BackColor = Color.Red;
                    //});
                    #endregion
                }
                else
                {
                    this.Invoke((EventHandler)delegate
                    {
                        libnodave.closeSocket(fds.rfd);
                        timer1.Enabled = true;
                        Thread.Sleep(100);
                        lblPLC.Text = "连接PLC失败" + DateTime.Now.ToString();
                        lblPLC.BackColor = Color.Red;
                    });

                    //MessageBox.Show("Couldn't open TCP connaction to ");
                    //  return -1;
                }
            }
            catch
            {

            }
            #region deleted
            // MessageBox.Show("end");
            //  libnodave.daveexecWriteRequest

            //  int rtVal = 1;
            //  short ConNr = 1; // First connection；(0 ... 63)；(max. 64 connections).
            //  string AccessPoint = "S7ONLINE"; // Default access point——S7ONLINE                  
            //  Prodave6_CS.Prodave6.CON_TABLE_TYPE ConTable;// Connection table
            //  int ConTableLen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Prodave6_CS.Prodave6.CON_TABLE_TYPE));// Length of the connection table

            //  ConTable.Adr = new byte[] { 192,168,2, 1, 0, 0 };
            //  ConTable.AdrType = 2;// (Byte)(globalVars.PSetting[9].iMode + 1); // Type of address: MPI/PB (1), IP (2), MAC (3)
            ///*  if (ConTable.AdrType > 3)
            //  {
            //      ConTable.AdrType = 3;
            //      ConTable.SlotNr = 1; // 插槽号
            //  }
            //  else
            //  {
            //      ConTable.SlotNr = 2;
            //  }
            //  ConTable.SlotNr = 1;
            //   * */
            //  ConTable.SlotNr = 1;
            //  ConTable.RackNr = 0; // 机架号  
            //  try
            //  {
            //      rtVal = Prodave6.LoadConnection_ex6(ConNr, AccessPoint, ConTableLen, ref ConTable);
            //  }
            //  catch// Exception(Ex)
            //  {
            //      //      MessageBox.Show("");
            //  }
            //  if (rtVal != 0)
            //  {
            //     globalVars.bPLCThread = false;
            //      return;
            //  }
            //  //以下测试SetActiveConnection_ex6
            //  UInt16 UConNr = (UInt16)ConNr;
            //  rtVal = Prodave6.SetActiveConnection_ex6(UConNr);
            //  while (globalVars.bPLCThread)
            //  {
            //      Thread.Sleep(200);
            //      UInt16 BlkNr = 1;//data block号
            //      UInt32 pDatLen = 0;
            //      Prodave6.DatType DType = Prodave6.DatType.BYTE;//要读取的数据类型
            //      //    DType = Prodave6.DatType.WORD;//要读取的数据类型
            //      UInt16 StartNr = 0;//起始地址号
            //      UInt32 pAmount = 10;//需要读取类型的数量 0-9是写  10-19是读
            //      UInt32 BufLen = 10;//缓冲区长度（字节为单位）
            //      //参数：data block号、要写入的数据类型、起始地址号、需要写入类型的数量、缓冲区长度（字节为单位）、缓冲区
            //      byte[] pWriteBuffer = new byte[20];
            //      rtVal = Prodave6.field_read_ex6(Prodave6.FieldType.D, 1, 10, pAmount, BufLen,globalVars.AGLBuff, ref pDatLen);
            //      globalVars.bConnState = (rtVal == 0);
            //      if ((globalVars.AGLBuff[0] & 1) != 0 & (globalVars.AGLBuffOld[0] & 1) == 0)
            //      {
            //          GC.Collect();
            //          trigCam();
            //      }

            //      Buffer.BlockCopy(globalVars.AGLBuff, 0, globalVars.AGLBuffOld, 0, globalVars.AGLBuff.Length);
            //      btnTest.Text=(rtVal.ToString()+ " "+globalVars.AGLBuff[0].ToString());           
            //  }

            //  Prodave6.UnloadConnection_ex6(UConNr);
            #endregion

        }
        public bool SaveToMDB(string sBC, string PartName, string SetID,int iOK)
        {
            if (connStr == null) return false;
            if (connStr.Length < 5) return false;
            OleDbConnection DBConn;
            OleDbCommand DBCommand;
            try
            {
                label19.Text = DateTime.Now.ToString() + ": \r\n" + sBC + " " + PartName + " " + SetID + "   OK=" + iOK.ToString();
                string strConnection = connStr;
                DBConn = new OleDbConnection(strConnection);
                DBConn.Open();
                DBCommand = DBConn.CreateCommand();
                DBCommand.CommandText = "insert into LaserParts(ID,Barcode,PartName,MachineNo,SetNum,isOK,Transdate) values(0,'" + sBC + "','" + PartName + "','" + StrMachineNo + "','" + SetID + "',"+ iOK.ToString() +",getdate())";
                DBCommand.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Err");
                //Log(e.Message);
                MessageBox.Show(e.Message);
                return false;
            }
            return false;
        }
        private void timerPLC_Tick(object sender, EventArgs e)
        {

        }

        private void lblPLC_Click(object sender, EventArgs e)
        {
            //ReadProdave();

            Thread myThread = new Thread(new ThreadStart(ReadProdave));
            //将线程设为后台运行
            myThread.IsBackground = true;
            myThread.Start();
        }

        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否确定停止采集A288数据", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bPLCThread = false;
                Thread.Sleep(1000);
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void Sensor1_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x100;
        }

        private void Sensor1_Click(object sender, EventArgs e)
        {

        }

        private void Sensor2_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x200;
        }

        private void Sensor3_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x400;
        }

        private void Sensor4_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x800;
        }

        private void Sensor7_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x4000;
        }

        private void Sensor8_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x8000;
        }

        private void Clamp2Close_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x1000;
        }

        private void Clamp2Open_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x2000;
        }

        private void Clamp1Close_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x1;
        }

        private void Clamp1Open_DoubleClick(object sender, EventArgs e)
        {
            iActionMask = 0x4;
        }

        private void label16_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure Change (1/2) parts number in one fixture ","", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
            else
            {
                return;
            }
            iActionMask_Parts = 1;
        }

        private void label23_Click(object sender, EventArgs e)
        {
           
        }

        private void label23_DoubleClick(object sender, EventArgs e)
        {

        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            lblPLC_Click(null, null);
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void lbFix_Click(object sender, EventArgs e)
        {
          //  SaveToMDB("aaaaa", "test a", "1.0.0", 1);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ig_Count = ig_Count + 1;
            lblNG.Text = ig_Count.ToString();
            if (ig_Count > 30)
            {
                ig_Count = 0;
                //timer1.Enabled = false;
                lblPLC_Click(null, null);
            }
        }

        private void lblCode_Click(object sender, EventArgs e)
        {
            ReadProdave_UW();
        }
    }
}
