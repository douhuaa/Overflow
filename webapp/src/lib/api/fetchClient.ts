export async function fetchClient<T>(
	url: string,
	method: 'GET' | 'POST' | 'PUT' | 'DELETE',
	options: Omit<RequestInit, 'body'> & { body?: unknown } = {}): Promise<T> {
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
	if (!response.ok) {
		const contentType = response.headers.get('content-type');
		const isJson =
			contentType?.includes('application/json')
			|| contentType?.includes('application/problem+json');
		const errorData = isJson ? await response.json() : await response.text();

		throw new Error(`HTTP ${response.status} ${response.statusText}: ${errorData || 'An error occurred'} (${method} ${apiUrl + url})`);

	}
	return response.json();
}