import {Question} from "@/lib/types";

const BASE_URL = 'http://localhost:8001';

export async function getQuestions(tag?: string): Promise<Question[]> {
	const url = new URL('/questions', BASE_URL);
	if (tag) url.searchParams.set('tag', tag);
	const response = await fetch(url.toString(), {
		cache: 'no-store',
		headers: {'Accept': 'application/json'}
	});
	if (!response.ok) throw new Error(response.statusText);
	return await response.json();
}

export async function getQuestion(id: string): Promise<Question | null> {
	const url = new URL(`/questions/${encodeURIComponent(id)}`, BASE_URL);
	const res = await fetch(url.toString(), {
		cache: 'no-store',
		headers: {'Accept': 'application/json'}
	});
	if (res.status === 404) return null;
	if (!res.ok) throw new Error(res.statusText);
	return await res.json();
}
