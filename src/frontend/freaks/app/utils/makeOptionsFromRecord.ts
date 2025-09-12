type Option<T extends number> = { value: T; label: string };

export function makeOptionsFromRecord<T extends number>(
  rec: Record<T, string>
): Option<T>[] {
  return (Object.keys(rec) as unknown as T[])
    .map((k) => ({ value: k, label: rec[k] }))
    .sort((a, b) => a.value - b.value);
}
