﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FMS_adapter
{
    public enum MODE { W, R, WR, E };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class FCB
    {
        public bool Loaded { get; set; }

        private IntPtr myFCBpointer;
       
        public FCB(IntPtr myFCBpointer)
        {
            this.myFCBpointer = myFCBpointer;
        }

        ~FCB()
        {
            if (myFCBpointer != null)
                cppToCsharpAdapter.deleteFcbObject(ref myFCBpointer);
        }

        public void closeFile()
        {
            try
            {
                cppToCsharpAdapter.closeFile(this.myFCBpointer);
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }

        public object readRecord(object dest, uint recNum)
        {
            try
            {

                IntPtr buffer;
                buffer = Marshal.AllocHGlobal(Marshal.SizeOf(dest.GetType()));

                cppToCsharpAdapter.readRecord(this.myFCBpointer, buffer, recNum);
                Marshal.PtrToStructure(buffer, dest);

                Marshal.FreeHGlobal(buffer);

                return dest;
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }

        public void addRecord(object source)
        {
            try
            {
                IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(source.GetType()));
                Marshal.StructureToPtr(source, buffer, true);

                cppToCsharpAdapter.addRecord(this.myFCBpointer, buffer);

                Marshal.FreeHGlobal(buffer);
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }

        //public void seekRec(uint from, int pos)
        //{
        //    try
        //    {
        //        cppToCsharpAdapter.seekRec(this.myFCBpointer, from, pos);
        //    }
        //    catch (SEHException)
        //    {
        //        IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
        //        string message = Marshal.PtrToStringAnsi(cString);
        //        throw new Exception(message);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public void updateRecCancel()
        {
            try
            {
                cppToCsharpAdapter.updateRecCancel(this.myFCBpointer);
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }

        public void deleteRecord(uint readForUpdate)
        {
            try
            {
                cppToCsharpAdapter.deleteRecord(this.myFCBpointer, readForUpdate);
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }

        public void updateRecord(object source)
        {
            try
            {
                IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(source.GetType()));
                Marshal.StructureToPtr(source, buffer, true);
                cppToCsharpAdapter.updateRecord(this.myFCBpointer, buffer);
                Marshal.FreeHGlobal(buffer);
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }

        public uint getRecInfoSize()
        {
            try
            {
                return cppToCsharpAdapter.getRecInfoSize(myFCBpointer);
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }

        public DirEntry getfileDesc()
        {
            try
            {
                DirEntry dirTemp;
                int structSize = Marshal.SizeOf(typeof(DirEntry)); //Marshal.SizeOf(typeof(Student)); 
                IntPtr buffer = Marshal.AllocHGlobal(structSize);

                dirTemp = new DirEntry();
                Marshal.StructureToPtr(dirTemp, buffer, true);
                cppToCsharpAdapter.getfileDesc(this.myFCBpointer, buffer);
                Marshal.PtrToStructure(buffer, dirTemp);
                Marshal.FreeHGlobal(buffer);
                return dirTemp;


            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
        }

        public RecEntry getRecEntry(int index)
        {
            try
            {
                RecEntry re = new RecEntry();
                int structSize = Marshal.SizeOf(re.GetType()); //Marshal.SizeOf(typeof(Student)); 
                IntPtr buffer = Marshal.AllocHGlobal(structSize);
                Marshal.StructureToPtr(re, buffer, true);

                // ... send buffer to dll
                cppToCsharpAdapter.getRecEntry(this.myFCBpointer, buffer, index);
                Marshal.PtrToStructure(buffer, re);

                // free allocate
                Marshal.FreeHGlobal(buffer);

                return re;
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastDiskErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }

        public List<RecEntry> getRecEntryList()
        {
            try
            {
                List<RecEntry> list = new List<RecEntry>();

               

                RecEntry recTemp;
                int structSize = Marshal.SizeOf(typeof(RecEntry)); //Marshal.SizeOf(typeof(Student)); 
                IntPtr buffer = Marshal.AllocHGlobal(structSize);
                // ... send buffer to dll
                int size = (int)getRecInfoSize();
                for (int i = 0; i < size; i++)
                {

                    
                    cppToCsharpAdapter.getRecEntry(this.myFCBpointer, buffer, i);
                    recTemp = new RecEntry();
                    Marshal.StructureToPtr(recTemp, buffer, true);
                    cppToCsharpAdapter.getRecEntry(this.myFCBpointer, buffer, i);
                    Marshal.PtrToStructure(buffer, recTemp);
                    list.Add(recTemp);
                }
                // free allocate
                Marshal.FreeHGlobal(buffer);
                return list;
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastDiskErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }

        }

        public uint getNumOfRecords()
        {
            try
            {
                return cppToCsharpAdapter.getNumOfRecords(myFCBpointer);
            }
            catch (SEHException)
            {
                IntPtr cString = cppToCsharpAdapter.getLastFcbErrorMessage(this.myFCBpointer);
                string message = Marshal.PtrToStringAnsi(cString);
                throw new Exception(message);
            }
            catch
            {
                throw;
            }
        }
        ////   Disk* d;
        //int path;
        //public int Path { get { return path; } }

        //DirEntry fileDesc;
        //public DirEntry FileDesc { get { return fileDesc; } }

        ////DATtype FAT;
        ////Sector buffer;
        //uint currRecNr;
        //public uint CurrRecNr { get { return currRecNr; } }

        //uint currSecNr;
        //public uint CurrSecNr { get { return currSecNr; } }

        //uint currRecNrInBuff;
        //public uint CurrRecNrInBuff { get { return currRecNrInBuff; } }

        //bool updateMode;
        //public bool UpdateMode { get { return updateMode; } }

        ////bool lock;
        ////public bool Lock { get { return lock; } }

        //MODE mode;
        //public MODE Mode { get { return mode; } }

        //uint numOfRecords;
        //public uint NumOfRecords { get { return numOfRecords; } }

        ////string lastErrorMessage;

        //int maxRecNum;
        //public int MaxRecNum { get { return maxRecNum; } }

        ////[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        ////int dat;
        ////public int DAT { get { return dat; } }

        //bool loaded;
        //public bool Loaded { get { return loaded; } }

        //RecInfo recInfo;
        //public RecInfo RecInfo { get { return recInfo; } }
    }
}
