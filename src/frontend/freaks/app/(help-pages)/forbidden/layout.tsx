import { ReactNode } from "react";

type Props = {
  children: ReactNode;
};

export default function ForbiddenLayout({ children }: Props) {
  return <main className="cleanContent">{children}</main>;
}
