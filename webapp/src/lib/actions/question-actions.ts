import {Question} from "@/lib/types";

const BASE_URL = 'http://localhost:8001';

export async function getQuestions(tag?: string): Promise<Question[]> {
	const url = new URL('/questions', BASE_URL);
	if (tag) url.searchParams.set('tag', tag);

	const response = await fetch(url);
	if (!response.ok) throw new Error(response.statusText);
	return await response.json();
}