'use client'
import {Input} from "@heroui/input";
import {MagnifyingGlassIcon} from "@heroicons/react/24/solid";
import {useEffect, useRef, useState} from "react";
import {Question} from "@/lib/types";
import {searchQuestions} from "@/lib/actions/question-actions";
import {Listbox, ListboxItem} from "@heroui/listbox";
import {Spinner} from "@heroui/spinner";

export default function SearchInput() {
	const [query, setQuery] = useState("");
	const [loading, setLoading] = useState(false);
	const [results, setResults] = useState<Question[] | null>(null);
	const timeoutRef = useRef<number | null>(null);

	// 根据状态派生是否展示下拉，不再维护独立的 showDropdown 状态
	const showDropdown = !!query && !loading && !!results?.length;

	useEffect(() => {
		// 清理上一次定时器
		if (timeoutRef.current !== null) {
			clearTimeout(timeoutRef.current);
			timeoutRef.current = null;
		}

		const trimmed = query.trim();
		if (!trimmed) {
			// 不在 effect 中同步 setState，清空逻辑已在 onChange 中处理
			return;
		}

		timeoutRef.current = window.setTimeout(async () => {
			setLoading(true);
			try {
				const {data: questions} = await searchQuestions(trimmed);
				setResults(questions);
			} finally {
				setLoading(false);
			}
		}, 300);

		// 组件卸载或依赖变更时清理
		return () => {
			if (timeoutRef.current !== null) {
				clearTimeout(timeoutRef.current);
				timeoutRef.current = null;
			}
		};
	}, [query]);

	const onAction = () => {
		setQuery("");
		setResults(null);
	};

	return (
		<div className="flex flex-col w-full">
			<Input
				startContent={<MagnifyingGlassIcon className="size-6"/>}
				className="ml-6"
				type="search"
				placeholder="Search"
				value={query}
				onChange={(e) => {
					const value = e.target.value;
					setQuery(value);
					// 输入被清空时在事件中同步清理，避免在 effect 中同步 setState
					if (!value) {
						setResults(null);
					}
				}}
				endContent={loading && <Spinner size="sm"/>}
			/>
			{showDropdown && (
				<div
					className="absolute top-full z-50 bg-white dark:bg-default-50 shadow-lg border-2 border-default-500 w-[50%]">
					<Listbox
						onAction={onAction}
						items={results ?? []}
						className="flex flex-col overflow-y-auto"
					>
						{(question) => (
							<ListboxItem
								href={`/questions/${question.id}`}
								key={question.id}
								startContent={
									<div
										className="flex flex-col h-14 min-w-14 justify-center items-center border border-success rounded-md">
										<span>{question.answerCount}</span>
										<span className="text-xs">answers</span>
									</div>
								}
							>
								<div>
									<div className="font-semibold">{question.title}</div>
									<div className="text-xs opacity-60 line-clamp-2">{question.content}</div>
								</div>
							</ListboxItem>
						)}
					</Listbox>
				</div>
			)}
		</div>
	);
}
