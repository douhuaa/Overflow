import {notFound} from "next/navigation";

export async function fetchClient<T>(
	url: string,
	method: 'GET' | 'POST' | 'PUT' | 'DELETE',
	options: Omit<RequestInit, 'body'> & { body?: unknown } = {})
	: Promise<{ data: T | null, error?: { message: string, status: number } }> {
	const {body, ...rest} = options;
	const apiUrl = process.env.API_URL ?? 'http://localhost:8001';
	const headers: HeadersInit = {
		'Content-Type': 'application/json',
		...(rest.headers || {})
	}
	const response = await fetch(apiUrl + url, {
		method,
		headers,
		...(body ? {body: JSON.stringify(body)} : {}),
		...rest
	})
	const contentType = response.headers.get('content-type');
	const isJson =
		contentType?.includes('application/json')
		|| contentType?.includes('application/problem+json');
	const parsed = isJson ? await response.json() : await response.text();	
	if (!response.ok) {
		if (response.status === 404) return notFound();
		let message = '';
		if (typeof parsed === 'string') {
			message = parsed
		} else if (parsed?.message) {
			message = parsed?.message;
		}

		if (!message) {
			message = getFallbackMessage(response.status);
		}
		return {data: null, error: {message, status: response.status}}
	}
	return {data: parsed as T};
}

function getFallbackMessage(status: number) {
	switch (status) {
		case 400:
			return "Bad Request. Please check your input.";
		case 401:
			return "You must be logged in.";
		case 403:
			return "You do not have permission to access this resource.";
		case 500:
			return "Server error. Please try again later}";
		default:
			return "Unknown error. Please try again later"
	}
}