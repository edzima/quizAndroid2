package md591606c899593bc34604895fc137b5056;


public class selectCategoryActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Quiz.selectCategoryActivity, Quiz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", selectCategoryActivity.class, __md_methods);
	}


	public selectCategoryActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == selectCategoryActivity.class)
			mono.android.TypeManager.Activate ("Quiz.selectCategoryActivity, Quiz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
