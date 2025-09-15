import styles from "./_styles.module.scss";

type Props = {
  children: React.ReactNode;
};

const DetailContainer = ({ children }: Props) => {
  return <div className={styles.detailContainer}>{children}</div>;
};

export default DetailContainer;
