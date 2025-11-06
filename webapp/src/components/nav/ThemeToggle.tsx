'use client'
import dynamic from "next/dynamic";
import {Button} from "@heroui/button";
import {useTheme} from "next-themes";
import {MoonIcon, SunIcon} from "@heroicons/react/24/solid";

function ThemeToggle() {
	const {resolvedTheme, setTheme} = useTheme();
	const isDark = resolvedTheme === 'dark';
	return (
		<Button
			color='primary'
			variant='light'
			isIconOnly
			aria-label="Toggle theme"
			onPress={() => setTheme(isDark ? 'light' : 'dark')}
		>
			{isDark ? (
				<MoonIcon className='h-8'/>
			) : (
				<SunIcon className='h-8 text-yellow-300'/>
			)}
		</Button>
	);
}

export default dynamic(() => Promise.resolve(ThemeToggle), {ssr: false});
