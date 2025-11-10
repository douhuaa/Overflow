import NextAuth from "next-auth"
import Keycloak from "next-auth/providers/keycloak"

export const {handlers, auth, signIn, signOut} = NextAuth({
	providers: [Keycloak],
	callbacks: {
		async jwt({token, account}) {
			if (account && account.access_token) {
				token.accessToken = account.access_token
			}
			return token;
		},
		async session({session, token}) {
			if (token) {
				session.accessToken = token.accessToken
			}

			return session;
		}
	}
})