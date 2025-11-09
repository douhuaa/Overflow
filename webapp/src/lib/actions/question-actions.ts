import {Question} from "@/lib/types";
import {fetchClient} from "@/lib/api/fetchClient";

export async function getQuestions(tag?: string): Promise<Question[]> {
	let url = "/questions";
	if (tag) url += `?tag=${tag}`;
	return fetchClient<Question[]>(url, 'GET')
}

export async function getQuestion(id: string): Promise<Question | null> {
	return fetchClient<Question>(`/questions/${id}`, 'GET')
}
