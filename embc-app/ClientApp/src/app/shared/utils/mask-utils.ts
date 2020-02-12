import { MaskGenerator } from '../interfaces/mask-generator';

export class MaskUtils {
    private static PHONE = '(999) 999-9999';
    private static DATE = 'YYYY-MM-DD';

    public static PHONE_MASK_GENERATOR: MaskGenerator = {
        generateMask: () => MaskUtils.PHONE
    }

    public static DATE_MASK_GENERATOR: MaskGenerator = {
        generateMask: () => MaskUtils.DATE
    }
    
}
