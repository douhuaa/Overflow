'use server';

import {fetchClient} from "@/lib/api/fetchClient";

export async function testAuth() {
	return fetchClient<string>(`/test/auth`, 'GET')
}

