import Image, { ImageProps, StaticImageData } from "next/image";

interface CustomImageProps extends ImageProps {
  src: string | StaticImageData;
  alt: string;
}

const CustomImage = ({ src, alt, ...props }: CustomImageProps) => {
  if (!src) return null;

  return (
    <Image
      src={src}
      alt={alt}
      fill
      sizes="(max-width: 1200px) 100vw, (max-width: 768px) 30vw, (max-width: 480px) 70vw"
      {...props}
    />
  );
};

export default CustomImage;
