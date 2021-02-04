using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    class ReceiveBuffer
    {
        ArraySegment<byte> _buffer;
        int _readPos;
        int _writePos;

        public ReceiveBuffer(int bufferSize)
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public int DataSize { get { return _writePos - _readPos; } }
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        public ArraySegment<byte> ReadSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
        }

        public ArraySegment<byte> WriteSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
        }

        /// <summary>
        /// Reposition buffer data to prevent overflow of buffer space
        /// </summary>
        public void Clean()
        {
            int dataSize = DataSize;
            if (DataSize.Equals(0))
            {
                // If no packet data is left, just reset the cursor position only
                _readPos = _writePos = 0; 
            }
            else
            {
                // Copy remaining packet data to init offset
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
            }
        }

        public bool OnRead(int numOfBytes)
        {
            if (numOfBytes > DataSize) return false;

            _readPos += numOfBytes;
            return true;
        }

        public bool OnWrite(int numOfBytes)
        {
            if (numOfBytes > FreeSize) return false;

            _writePos += numOfBytes;
            return true;
        }
    }
}
