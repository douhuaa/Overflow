'use client';

import {ReactNode} from "react";
import {HeroUIProvider} from "@heroui/react";

export default function Providers({children}: { children: ReactNode }) {
	return (
		<HeroUIProvider className={'h-full flex flex-col'}>
			{children}
		</HeroUIProvider>
	);
}
