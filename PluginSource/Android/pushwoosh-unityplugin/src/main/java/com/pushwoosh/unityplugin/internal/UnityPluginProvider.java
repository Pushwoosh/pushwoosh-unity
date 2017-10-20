package com.pushwoosh.unityplugin.internal;

import com.pushwoosh.internal.PluginProvider;

public class UnityPluginProvider implements PluginProvider {
	@Override
	public String getPluginType() {
		return "Unity";
	}

	@Override
	public int richMediaStartDelay() {
		return 500;
	}
}
