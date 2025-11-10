'use server';


import {fetchClient} from "@/lib/api/fetchClient";
import {Tag} from "@/lib/types";
import {auth} from "@/auth";
import {getSession} from "next-auth/react";

export async function getTags() {
	return fetchClient<Tag[]>('/tags', 'GET', {cache: 'force-cache', next: {revalidate: 3600}})
}

export async function getCurrentUser() {
	try {
		const session = await auth();
		if (!session) return null;
		return session.user;
	} catch (e) {
		console.error(e);
		return null;
	}
}