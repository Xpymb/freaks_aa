import { redirect } from "next/navigation";
import { SiteMetaData } from "./enum";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: SiteMetaData.title,
  description: SiteMetaData.description,
};

export default function HomePage() {
  redirect("/overview");
}
