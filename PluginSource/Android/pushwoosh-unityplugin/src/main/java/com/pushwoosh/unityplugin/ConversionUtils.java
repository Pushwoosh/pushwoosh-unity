package com.pushwoosh.unityplugin;

import java.util.Iterator;
import java.util.List;
import java.util.Map;

import com.pushwoosh.internal.utils.PWLog;
import com.pushwoosh.tags.TagsBundle;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;


import static android.R.attr.key;

public class ConversionUtils {
	public static TagsBundle convertToTagsBundle(Map<String, Object> readableMap) {
		return new TagsBundle.Builder()
				.putAll(toJsonObject(readableMap))
				.build();
	}

	private static JSONObject toJsonObject(Map<String, Object> readableMap) {
		final Iterator<Map.Entry<String, Object>> iterator = readableMap.entrySet().iterator();
		JSONObject result = new JSONObject();
		while (iterator.hasNext()) {
			final Map.Entry<String, Object> entry = iterator.next();
			String key = entry.getKey();
			Object value = entry.getValue();
			try {
				if (value == null) {
					result.put(key, JSONObject.NULL);
				} else if (value instanceof Map) {
					result.put(key, toJsonObject((Map) value));
				} else if (value instanceof List) {
					result.put(key, toArray((List) value));
				} else if (value instanceof Boolean) {
					result.put(key, (Boolean) value);
				} else if (value instanceof Integer) {
					result.put(key, (Integer) value);
				} else if (value instanceof String) {
					result.put(key, (String) value);
				} else if (value instanceof Double) {
					result.put(key, (Double) value);
				} else {
					result.put(key, value);
				}

			} catch (JSONException e) {
				PWLog.error(PushwooshProxy.TAG, "Could not convert object with key: " + key + ".", e);
			}

		}
		return result;
	}

	public static JSONArray toArray(List<Object> readableArray) {
		JSONArray result = new JSONArray();
		for (int i = 0; i < readableArray.size(); i++) {
			Object value = readableArray.get(i);
			try {
				if (value == null) {
					result.put(i, JSONObject.NULL);
				} else if (value instanceof Map) {
					result.put(i, toJsonObject((Map) value));
				} else if (value instanceof List) {
					result.put(i, toArray((List) value));
				} else if (value instanceof Boolean) {
					result.put(i, (Boolean) value);
				} else if (value instanceof Integer) {
					result.put(i, (Integer) value);
				} else if (value instanceof String) {
					result.put(i, (String) value);
				} else if (value instanceof Double) {
					result.put(i, (Double) value);
				} else {
					result.put(i, value);
				}

			} catch (JSONException e) {
				PWLog.error(PushwooshProxy.TAG, "Could not convert object with key: " + key + ".", e);
			}
		}
		return result;
	}
}
