using System.Collections.Generic;
using System.Linq;

class LocalQueue<Tem> {
	public LinkedList<Tem> tem = new LinkedList<Tem>();
	public bool IsEmpty() {
		return tem.Count <= 0;
	}
	public Tem Pop() {
		Tem ret = tem.First();
		tem.RemoveFirst();
		return ret;
	}
	public void Push(Tem t) {
		tem.AddLast(t);
	}
}
