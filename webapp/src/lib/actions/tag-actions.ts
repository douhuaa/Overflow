'use server';


import {fetchClient} from "@/lib/api/fetchClient";
import {Tag} from "@/lib/types";

export async function getTags() {
	return fetchClient<Tag[]>('/tags', 'GET')
}