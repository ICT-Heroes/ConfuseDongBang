using System;

namespace PenguinModel
{
	public class FailInfo
	{
		public enum FailType {
			NotExist, ServerAccessFailed, Etc
		};
		public FailType failType;
		public string errorCode;

		public FailInfo (FailType failType, string errorCode)
		{
			this.failType = failType;
			this.errorCode = errorCode;
		}
	}
}

