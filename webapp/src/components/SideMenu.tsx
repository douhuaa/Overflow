'use client'
import {HomeIcon, QuestionMarkCircleIcon, TagIcon, UserIcon} from "@heroicons/react/24/solid";
import {Listbox, ListboxItem} from "@heroui/listbox";
import {usePathname} from "next/navigation";
import clsx from "clsx";

export default function SideMenu() {
	const pathname = usePathname();
	const navLinks = [
		{key: 'home', icon: HomeIcon, text: 'Home', href: '/'},
		{key: 'questions', icon: QuestionMarkCircleIcon, text: 'Questions', href: '/questions'},
		{key: 'tags', icon: TagIcon, text: 'Tags', href: '/tags'},
		{key: 'session', icon: UserIcon, text: 'User Session', href: '/session'},
	];

	const isActive = (href: string) => href === '/'
		? pathname === '/'
		: pathname === href || pathname.startsWith(href + '/');

	const activeKey = (navLinks.find(l => isActive(l.href))?.key) ?? 'home';

	return (
		<Listbox
			aria-label={'nav links'}
			variant={'faded'}
			items={navLinks}
			className={'sticky top-20 ml-6'}
			selectionMode={'single'}
			selectedKeys={new Set([activeKey])}
		>
			{({key, href, icon: Icon, text}) => (
				<ListboxItem
					key={key}
					href={href}
					startContent={<Icon className='h-6'/>}
					classNames={{
						base: 'data-[hover=true]:bg-default-100',
						title: clsx('text-lg', isActive(href) && 'text-secondary')
					}}
				>
					{text}
				</ListboxItem>
			)}
		</Listbox>
	);
}
