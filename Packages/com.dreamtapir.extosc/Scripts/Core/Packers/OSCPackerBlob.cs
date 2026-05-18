/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using System;

namespace extOSC.Core.Packers
{
	internal class OSCPackerBlob : OSCPacker<byte[]>
	{
		#region Public Methods

		public override OSCValueType PackerType => OSCValueType.Blob;

		#endregion

		#region Private Vars

		private readonly byte[] _data = new byte[sizeof(int)];

		#endregion

		#region Protected Methods

		protected override byte[] BytesToValue(byte[] buffer, ref int index)
		{
			_data[0] = buffer[index++];
			_data[1] = buffer[index++];
			_data[2] = buffer[index++];
			_data[3] = buffer[index++];

			if (BitConverter.IsLittleEndian)
				Array.Reverse(_data);

			var blobSize = BitConverter.ToInt32(_data, 0);
			var blob = new byte[blobSize];

			Array.Copy(buffer, index, blob, 0, blobSize);

			index += blobSize;
			SkipBlobPadding(blobSize, ref index);

			return blob;
		}

		protected override void ValueToBytes(byte[] buffer, ref int index, byte[] value)
		{
			var bytes = BitConverter.GetBytes(value.Length);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			buffer[index++] = bytes[0];
			buffer[index++] = bytes[1];
			buffer[index++] = bytes[2];
			buffer[index++] = bytes[3];

			Array.Copy(value, 0, buffer, index, value.Length);

			index += value.Length;
			WriteBlobPadding(buffer, value.Length, ref index);
		}

		#endregion
	}
}