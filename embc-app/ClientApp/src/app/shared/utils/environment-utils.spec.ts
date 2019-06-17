import { ieVersion } from './environment-utils';

describe('ieVersion()', () => {
  it('returns NULL for Chrome', () => {
    const version = ieVersion('Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.90 Safari/537.36');
    expect(version).toBeNull();
  });

  it('returns 10 for Internet Explorer 10', () => {
    const version = ieVersion('Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)');
    expect(version).toEqual(10);
  });

  it('returns 11 for Internet Explorer 11', () => {
    const version = ieVersion('Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko');
    expect(version).toEqual(11);
  });

  it('returns 12 for Edge 12', () => {
    const version = ieVersion('Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36 Edge/12.0');
    expect(version).toEqual(12);
  });

  it('returns 13 for Edge 13', () => {
    const version = ieVersion('Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586');
    expect(version).toEqual(13);
  });
});
