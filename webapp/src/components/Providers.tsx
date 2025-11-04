'use client';

import {ReactNode} from "react";
import {HeroUIProvider} from "@heroui/react";
import {useRouter} from "next/navigation";


export default function Providers({children}: { children: ReactNode }) {
	const router = useRouter();
	return (
		<HeroUIProvider navigate={router.push} className={'h-full flex flex-col'}>
			{children}
		</HeroUIProvider>
	);
}
