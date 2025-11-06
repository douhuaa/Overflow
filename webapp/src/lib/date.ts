export function formatIsoToLocal(iso: string): string {
	const d = new Date(iso);
	if (Number.isNaN(d.getTime())) return iso;
	const pad = (n: number) => String(n).padStart(2, "0");
	const y = d.getFullYear();
	const m = pad(d.getMonth() + 1);
	const day = pad(d.getDate());
	const h = pad(d.getHours());
	const min = pad(d.getMinutes());
	const s = pad(d.getSeconds());
	return `${y}-${m}-${day} ${h}:${min}:${s}`;
}

