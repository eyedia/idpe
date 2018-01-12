#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.
 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#region Developer Information
/*
Author  - Deb'jyoti Das
Created - 3/19/2013 11:14:16 AM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;

namespace Eyedia.IDPE.Interface.Controls
{
#if false

    // This version of the PageImageList stores images as byte arrays. It is a little
    // more complex and slower than a simple list, but doesn't consume GDI resources.
    // This is important when the list contains lots of images (Windows only supports
    // 10,000 simultaneous GDI objects!)
    class PageImageList
    {
        // ** fields
        List<byte[]> _list = new List<byte[]>();

        // ** object model
        public void Clear()
        {
            _list.Clear();
        }
        public int Count
        {
            get { return _list.Count; }
        }
        public void Add(Image img)
        {
            _list.Add(GetBytes(img));

            // stored image data, now dispose of original
            img.Dispose();
        }
        public Image this[int index]
        {
            get { return GetImage(_list[index]); }
            set { _list[index] = GetBytes(value); }
        }

        // implementation
        byte[] GetBytes(Image img)
        {
            // use interop to get the metafile bits
            Metafile mf = img as Metafile;
            var enhMetafileHandle = mf.GetHenhmetafile().ToInt32();
            var bufferSize = GetEnhMetaFileBits(enhMetafileHandle, 0, null);
            var buffer = new byte[bufferSize];
            GetEnhMetaFileBits(enhMetafileHandle, bufferSize, buffer);

            // return bits
            return buffer;
        }
        Image GetImage(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            return Image.FromStream(ms);
        }

        [System.Runtime.InteropServices.DllImport("gdi32")]
        static extern int GetEnhMetaFileBits(int hemf, int cbBuffer, byte[] lpbBuffer);
    }

#else

    // This version of the PageImageList is a simple List<Image>. It is simple,
    // but caches one image (GDI object) per preview page.
    class PageImageList : List<Image>
    {
    }

#endif
}


