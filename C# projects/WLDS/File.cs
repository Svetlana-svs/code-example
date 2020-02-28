using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace Wlds
{
    class File
    {
        public static HRESULT ReadFile(string fName, out float[] bufData)
        {
            bufData = null;
            try
            {
                if (System.IO.File.Exists(fName))
                {
                    FileStream fStream = new FileStream(fName, FileMode.Open, System.IO.FileAccess.Read);
                    BinaryReader reader = new BinaryReader(fStream);
                    long fLength = fStream.Length / sizeof(float);
                    bufData = new float[fLength];

                    for (int i = 0; i < fLength; i++)
                    {
                        bufData[i] = reader.ReadSingle();
                    }

                    fStream.Close();
                    fStream.Dispose();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error by reading from file. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            return HRESULT.S_OK;
        }
    }
}
