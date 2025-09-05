import { useCallback, useState } from "react";

export function useDisclosure(initial = false) {
  const [open, setOpen] = useState(initial);
  const onOpen = useCallback(() => setOpen(true), []);
  const onClose = useCallback(() => setOpen(false), []);
  const onToggle = useCallback(() => setOpen((v) => !v), []);
  return { open, onOpen, onClose, onToggle, setOpen };
}
